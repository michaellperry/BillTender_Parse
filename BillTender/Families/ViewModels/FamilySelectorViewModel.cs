using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BillTender.Families.Models;
using BillTender.Helpers;
using BillTender.ViewModels;
using Parse;
using UpdateControls.XAML;
using Windows.Storage;
using System.Xml.Serialization;
using System.Threading.Tasks;
using BillTender.Families.Mementos;

namespace BillTender.Families.ViewModels
{
    public class FamilySelectorViewModel : ProgressViewModel
    {
        private readonly ParseUser _user;
        private readonly FamilySelectionModel _familySelection;

        public FamilySelectorViewModel(ParseUser user, FamilySelectionModel familySelection)
        {
            _user = user;
            _familySelection = familySelection;
        }

        public void Load()
        {
            Perform(async delegate
            {
                var familiesCached = await LoadFamiliesAsync(
                    _user.ObjectId);
                _familySelection.SetFamilies(familiesCached);

                var families = await QueryFamiliesAsync(_user);
                _familySelection.SetFamilies(families);

                await SaveFamiliesAsync(
                    _user.ObjectId,
                    families);
            });
        }

        public IEnumerable<Family> Families
        {
            get { return _familySelection.Families; }
        }

        public Family SelectedFamily
        {
            get { return _familySelection.SelectedFamily; }
            set { _familySelection.SelectedFamily = value; }
        }

        public ICommand AddFamily
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        Family family = ParseObject.Create<Family>();
                        DialogManager.ShowFamilyDialog(
                            "New Family",
                            family,
                            completed: delegate
                            {
                                Perform(async delegate
                                {
                                    await family.SaveAsync();
                                    family.Initialize();
                                    family.Writers.Users.Add(_user);
                                    await family.SaveAsync();

                                    _familySelection.AddFamily(family);
                                    _familySelection.SelectedFamily = family;
                                });
                            });
                    });
            }
        }

        public ICommand EditFamily
        {
            get
            {
                return MakeCommand
                    .When(() => _familySelection.SelectedFamily != null)
                    .Do(delegate
                    {
                        Family selectedFamily = _familySelection.SelectedFamily;
                        DialogManager.ShowFamilyDialog(
                            "Edit Family",
                            selectedFamily,
                            completed: delegate
                            {
                                Perform(async delegate
                                {
                                    await selectedFamily.SaveAsync();
                                });
                            },
                            cancelled: delegate
                            {
                                selectedFamily.Revert();
                            });
                    });
            }
        }

        public ICommand RemoveFamily
        {
            get
            {
                return MakeCommand
                    .When(() => _familySelection.SelectedFamily != null)
                    .Do(delegate
                    {
                        Perform(async delegate
                        {
                            Family selectedFamily = _familySelection.SelectedFamily;

                            // TODO
                            selectedFamily.Readers.Users.Remove(_user);
                            selectedFamily.Writers.Users.Remove(_user);
                            await selectedFamily.SaveAsync();

                            _familySelection.RemoveFamily(selectedFamily);
                            _familySelection.SelectedFamily =
                                _familySelection.Families.FirstOrDefault();
                        });
                    });
            }
        }

        private static XmlSerializer FamilySerializer =
            new XmlSerializer(typeof(FamilyListMemento));

        private static async Task<List<Family>> LoadFamiliesAsync(
            string userId)
        {
            try
            {
                string fileName = userId + ".xml";
                var folder = await ApplicationData.Current.LocalFolder
                    .GetFolderAsync("Families");
                var file = await folder.GetFileAsync(fileName);
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    var familyList = (FamilyListMemento)FamilySerializer
                        .Deserialize(stream);
                    return familyList.Families
                        .Select(memento => Family.FromMemento(memento))
                        .ToList();
                }
            }
            catch (Exception x)
            {
                return new List<Family>();
            }
        }

        private static async Task SaveFamiliesAsync(
            string userId,
            List<Family> families)
        {
            string fileName = userId + ".xml";
            var folder = await ApplicationData.Current.LocalFolder
                .CreateFolderAsync(
                    "Families",
                    CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync(
                fileName,
                CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                var memento = new FamilyListMemento
                {
                    Families = families
                        .Select(family => family.ToMemento())
                        .ToList()
                };
                FamilySerializer.Serialize(stream, memento);
            }
        }

        private async static Task<List<Family>> QueryFamiliesAsync(
            ParseUser user)
        {
            var roles =
                from role in new ParseQuery<ParseRole>()
                where role["users"] == user
                select role;
            var writable =
                from writer in roles
                join family in new ParseQuery<Family>()
                    on writer equals family.Writers
                select family;
            var readable =
                from reader in roles
                join family in new ParseQuery<Family>()
                    on reader equals family.Readers
                select family;
            var families = await writable.Or(readable)
                .Include("Readers")
                .Include("Writers")
                .FindAsync();
            return families.ToList();
        }
    }
}
