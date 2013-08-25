BillTender_Parse
================

Bill Tender is an example Windows Store application written for several mobile backends. This is
the version for Parse.

Sign up for a free Parse accout at http://parse.com. Fork this project and open it in Visual Studio.

Double-click the file ParseConfig_Keys.cs to create it. This file is in .gitignore so it won't be
accidentally committed. Follow the instructions in ParseConfig.cs to enter your developer and application
keys into the file.

## Project structure

This project uses Update Controls as the MVVM framework. The foundation of the application is the Model layer.

### Models

The LoginModel keeps track of the currently logged in user. Parse represents the user with the ParseUser class.
It returns the currently logged in user from a static method. The LoginModel simply encapsulates this method
and provides an Independent tracking proxy. When a user signs up, logs in, or logs out, the LoginModel calls
OnSet on this tracking proxy to indicate that a change has occurred.

The LoginModel also keeps track of whether the user is signing up or logging in, so that the correct UI can
be displayed. Finally, it keeps track of whether the sign up or login process is busy, and what exception
was last thrown from either of these processes.

### ViewModels

The ViewModelLocator returns an instance of the main, sign up, log in, and account view models. Each of these
view models controls a different section of the UI. All view models share a single instance of the LoginModel,
providing a consistent experience across the views.

The MainViewModel selects the visual state based on the login status of the user. If a user is logged in, then
it selects the LoggedIn state. If not, it selects either LoggingIn or SigningUp, depending upon the flag in
the LoginModel.

The LogInViewModel controls the log in view. If the user presses the "Create Account" button, it clears the
ExistingAccount flag in the LoginModel, so that the signup view will be displayed instead.

The SignUpViewModel controls the sign up view. If the user presses the "Existing Account" button, it sets the
ExistingAccount flag.

The AccountViewModel returns information about the currently logged in user. It also responds to the LogOut
button by calling the Parse LogOut method, and notifying the LoginModel of the change.

None of the view models have properties for the user name or password. Security best practices require
that the password not be data bound or sent to the view model.

### Views

The LogInControl is a user control that binds to the LogInViewModel. It provides the log in UI. The user
name and password are stored only in the controls, and not data bound to the view model. The view is
responsible for calling the Parse LogIn method and passing in these values.

The SignUpControl is a user control that binds to the SignUpViewModel. It provides the sign up UI. This
view is responsible for calling the Parse SignUp method and passing in the user name and password.

The AccountControl is a user control that binds to the AccountViewModel. It appers as a text button showing
the user name, and opens a popup with the log out button when the user clicks on it.

### MainPage.xaml

The MainPage aggregates all of the views. It binds to the MainViewModel to control the visual state manager,
thus selecting which controls are visible.

## Next Steps

So far, the application only calls a handfull of Parse methods. It sets up a user account, and logs the
user in and out.

The next step is to store personal information on the ParseUser object. This will include things like name,
preferences, and settings.

Then we will create other ParseObject structures to capture application data, like payment schedules,
payment history, and reminders. We will relate these objects to the ParseUser and assign the approprate
access restrictions.

Next, we will share these objects with other users. We will define a mechanism for a user to identify others
with whom to share. We will also define a hub concept so that users can discover one another based on
a common interest. For example, they can discover other users paying the same providers, and compare rates.

And then we will take advantage of push notifications. The program will remind the user when a bill is coming due.

Finally, we will attempt to make the program work correctly while off line. We will examine patterns for
caching, updating, and queuing.
