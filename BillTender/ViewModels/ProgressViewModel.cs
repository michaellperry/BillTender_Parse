using System;
using System.Threading.Tasks;
using UpdateControls.Fields;

namespace BillTender.ViewModels
{
    public abstract class ProgressViewModel
    {
        private Independent<string> _lastError = new Independent<string>();
        private Independent<bool> _busy = new Independent<bool>();

        public string LastError
        {
            get { return _lastError; }
            set { _lastError.Value = value; }
        }

        public bool Busy
        {
            get { return _busy; }
        }

        protected async void Perform(Func<Task> asyncAction)
        {
            try
            {
                _busy.Value = true;
                _lastError.Value = string.Empty;
                await asyncAction();
            }
            catch (Exception x)
            {
                _lastError.Value = x.Message;
            }
            finally
            {
                _busy.Value = false;
            }
        }
    }
}
