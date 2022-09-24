using System;
using System.Collections.Generic;

namespace Planday.Schedule.Tests.MockData
{
    class ShiftMockData
    {
        public static List<Shift> GetShifts()
        {
            return new List<Shift>{
                new (1, 1, DateTime.Parse("2022-09-19 12:00:00.000"), DateTime.Parse("2022-09-19 17:00:00.000")),
                new (2,  2, DateTime.Parse("2022-09-19 09:00:00.00"), DateTime.Parse("2022-09-19 15:00:00.000")),
                 new (3,  null, DateTime.Parse("2022-09-19 08:00:00.00"), DateTime.Parse("2022-09-19 09:00:00.000"))
            };
        }
        public static List<Shift> GetShiftsWithUnassignedShift()
        {
            return new List<Shift>{
                new (1, 1, DateTime.Parse("2022-09-19 12:00:00.000"), DateTime.Parse("2022-09-19 17:00:00.000")),
                new (2,  2, DateTime.Parse("2022-09-19 09:00:00.00"), DateTime.Parse("2022-09-19 15:00:00.000")),
                new (3,  null, DateTime.Parse("2022-09-20 15:00:00.00"), DateTime.Parse("2022-09-20 18:00:00.000"))
            };
        }
        public static List<Shift> GetShiftsWithUnassignedShiftOverlapping()
        {
            return new List<Shift>{
                new (1, 1, DateTime.Parse("2022-09-19 12:00:00.000"), DateTime.Parse("2022-09-19 17:00:00.000")),
                new (2,  2, DateTime.Parse("2022-09-19 09:00:00.00"), DateTime.Parse("2022-09-19 15:00:00.000")),
                new (3,  null, DateTime.Parse("2022-09-19 13:00:00.00"), DateTime.Parse("2022-09-19 18:00:00.000"))
            };
        }
        public static List<Shift> GetShiftsWithAssignedShift()
        {
            return new List<Shift>{
                new (1, 1, DateTime.Parse("2022-09-19 12:00:00.000"), DateTime.Parse("2022-09-19 17:00:00.000")),
                new (2,  2, DateTime.Parse("2022-09-19 09:00:00.00"), DateTime.Parse("2022-09-19 15:00:00.000")),
                new (3,  2, DateTime.Parse("2022-09-20 09:00:00.00"), DateTime.Parse("2022-09-20 11:00:00.000")),
            };
        }
    }
}
