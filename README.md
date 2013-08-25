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
