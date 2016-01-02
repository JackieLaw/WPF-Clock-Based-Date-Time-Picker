namespace PeteEvans.PickerComponents
{
    /// <summary>
    ///     Constants for time pickers
    /// </summary>
    public static class DateTimePickerConstants
    {
        //
        // Clock face sizes
        //

        // Clock face sizes
        public const double ClockFaceOuterSize = 300;
        public const double ClockFaceInnerSize = 280;

        // 12 hour highlight ring
        public const double OuterHourHighlightDelta = 4; // Gap from clock face to the highlight of the hours
        public const double OuterHourHighlightSize = ClockFaceInnerSize - 2*OuterHourHighlightDelta;
        public const double OuterHourHighlightThickness = 13;

        // 12 hour clock elements
        public const double OuterHourDelta = 0; // Gap from clock face to the hours
        public const double OuterHourSize = ClockFaceInnerSize - 2*OuterHourDelta;
        public const double OuterHourThickness = 13; // Estimated size of hour elements

        // Minute highlight ring
        public const double MinuteHighlightDelta = 2; // Gap from the Outer hours highlight to the minutes highlight

        public const double MinuteHighlightCumulativeDelta =
            OuterHourHighlightDelta + OuterHourHighlightThickness + MinuteHighlightDelta;

        public const double MinuteHighlightSize = ClockFaceInnerSize - 2*MinuteHighlightCumulativeDelta;
        public const double MinuteHighlightThickness = 13;

        // Minute clock elements
        public const double MinuteDelta = 11; // Gap from the Outer hour elements to minutes
        public const double MinuteCumulativeDelta = OuterHourDelta + OuterHourThickness + MinuteDelta;
        public const double MinuteSize = ClockFaceInnerSize - 2*MinuteCumulativeDelta;
        public const double MinuteThickness = 6; // Estimated size of minute elements

        public const double MinuteMarkSize = 3;

        // 24 hour highlight ring
        public const double InnerHourHighlightDelta = 2; // Gap from clock face to the highlight of the hours

        public const double InnerHourHighlightCumulativeDelta =
            MinuteHighlightCumulativeDelta + MinuteHighlightThickness + InnerHourHighlightDelta;

        public const double InnerHourHighlightSize = ClockFaceInnerSize - 2*InnerHourHighlightCumulativeDelta;
        public const double InnerHourHighlightThickness = 13;

        // 24 hour clock elements
        public const double InnerHourDelta = 5; // Gap from clock face to the inner hour elements
        public const double InnerHourCumulativeDelta = MinuteCumulativeDelta + MinuteThickness + InnerHourDelta;
        public const double InnerHourSize = ClockFaceInnerSize - 2*InnerHourCumulativeDelta;
        public const double InnerHourThickness = 9;

        //
        // Hand Sizes
        // NB. Top + CentreX and Left + CentreY should equal 140 (half the height)
        //

        // Minute Hand
        public const double MinuteHandPositiveLength = 92; // From centre to end of hand
        public const double MinuteHandNegativeLength = 10; // From centre to the reverse end of the hand
        public const double MinuteHandTotalLength = MinuteHandPositiveLength + MinuteHandNegativeLength;
        public const double MinuteHandLengthOffset = ClockFaceInnerSize/2 - MinuteHandPositiveLength;

        public const double MinuteHandHalfWidth = 3;
        public const double MinuteHandWidth = MinuteHandHalfWidth*2;
        public const double MinuteHandWidthOffset = ClockFaceInnerSize/2 - MinuteHandHalfWidth;

        public const double MinuteHandMarkOffset = 5; // Distance from end of hand
        public const double MinuteHandMarkLength = 15;
        public const double MinuteHandMarkApparentLength = MinuteHandPositiveLength - MinuteHandMarkOffset;
        public const double MinuteHandMarkLengthOffet = ClockFaceInnerSize/2 - MinuteHandMarkApparentLength;

        public const double MinuteHandMarkHalfWidth = 1;
        public const double MinuteHandMarkWidth = MinuteHandMarkHalfWidth*2;
        public const double MinuteHandMarkWidthOffset = ClockFaceInnerSize/2 - MinuteHandMarkHalfWidth;

        public const double MinuteHandGrabHalfWidth = 4;
        public const double MinuteHandGrabWidth = MinuteHandGrabHalfWidth*2;
        public const double MinuteHandGrabWidthOffset = ClockFaceInnerSize/2 - MinuteHandGrabHalfWidth;

        // Hour Hand
        public const double HourHandPositiveLength = 75; // From centre to end of hand
        public const double HourHandNegativeLength = 8; // From centre to the reverse end of the hand
        public const double HourHandTotalLength = HourHandPositiveLength + HourHandNegativeLength;
        public const double HourHandLengthOffset = ClockFaceInnerSize/2 - HourHandPositiveLength;

        public const double HourHandHalfWidth = 4;
        public const double HourHandWidth = HourHandHalfWidth*2;
        public const double HourHandWidthOffset = ClockFaceInnerSize/2 - HourHandHalfWidth;

        public const double HourHandMarkOffset = 5; // Distance from end of hand
        public const double HourHandMarkLength = 10;
        public const double HourHandMarkApparentLength = HourHandPositiveLength - HourHandMarkOffset;
        public const double HourHandMarkLengthOffet = ClockFaceInnerSize/2 - HourHandMarkApparentLength;

        public const double HourHandMarkHalfWidth = 2;
        public const double HourHandMarkWidth = HourHandMarkHalfWidth*2;
        public const double HourHandMarkWidthOffset = ClockFaceInnerSize/2 - HourHandMarkHalfWidth;

        public const double HourHandGrabHalfWidth = 4;
        public const double HourHandGrabWidth = HourHandGrabHalfWidth*2;
        public const double HourHandGrabWidthOffset = ClockFaceInnerSize/2 - HourHandGrabHalfWidth;

        //
        // Radial Time Picker Sizes
        //
        public const double DatePickerOuterSize = 300;
        public const double DatePickerInnerSize = 280;

        public const double DatePickerDaysThisMonthMargin = 5;
        public const double DatePickerDaysThisMonthSize = DatePickerInnerSize - 2*DatePickerDaysThisMonthMargin;

        public const double DatePickerDaysNextMonthMargin = 25;
        public const double DatePickerDaysNextMonthSize = DatePickerInnerSize - 2*DatePickerDaysNextMonthMargin;

        public const double DatePickerMonthMargin = 40;
        public const double DatePickerMonthSize = DatePickerInnerSize - 2*DatePickerMonthMargin;

        public const double DatePickerMonthNextYearMargin = 79;
        public const double DatePickerMonthNextYearSize = DatePickerInnerSize - 2*DatePickerMonthNextYearMargin;

        public const double DatePickerYearMargin = 100;
        public const double DatePickerYearSize = DatePickerInnerSize - 2*DatePickerYearMargin;
    }
}