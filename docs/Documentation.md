# Documentation

## Requirements

This was implemented in Visual Studio 2013 using dotNet 4.5.
It has not been tested with earlier versions of Visual Studio or earlier version of the dotNet framework.

## Demonstration Application

The source code contains a demonstration application in the DateTimePickerDemo project.
Ensure this is the startup project and run the application to see examples usage of the date time picker.

## Control Usage

Both the NullableDateTimePicker and DateTimePicker have the following DependencyProperties:

* SelectedValue (the selected date time only changes on completion of date time selection)
* InterimValue (the value currently set in the popup, changes during date time selection)
* Format (string used to control the format of the date time displayed in the control)

## Code Structure

There are two projects in the solution:

* DateTimePickerDemo (the demonstration application)
* DateTimePicker (the DLL containing the controls)

The DateTimePicker package structure is as follows:

* AutoCopmpletionComboBox (implementation of the combo box used to select date and time based on text entry)
* Extenstions (some Date Time extension functions)
* PickerComponents (the individual controls used to implement the Date Time Pickers)
* Pickers (the actual Date Time Picker controls)
* RadialPanel (the radial panel used for the clock numbers and date picker buttons)

## Main Classes

The interesting files/classes are:

* DateTimePicker (the non-nullable Date Time Picker)
* NullableDateTimePicker (the nullable Date Time Picker)

* AutoCompletionDatePicker (supports date selection based on textual input)
* AutoCompletionTimePicker (supports time of day selection based on textual input)
* GraphicalTimePicker (supports clock based selection of time of day)
* RadialDatePickerControl (supports selection of date using the mouse)

* AutoCompletionComboBox (base control to allow implementation of suggestion based selections)
* DateSuggestionProvider (suggests possible date completions for absolute date entry)
* TimeSuggetionProvider (suggests possible time of day completions for absolute time entry)
* Relative Date Suggestion Provider (suggests possible date completions based on relative date entry e.g + 10y 3m 13d)
* RelativeTimeSuggestionProvider (suggests possible time completions based on relative time entry e.g. - 1h 15m)

* RadialPanel (the radial panel used for clock numbers and date picker buttons)

## Extensibility

New controls should be fairly easy to build using the components.
New suggestion providers (for textual entry) can be written and plugged in fairly simply.