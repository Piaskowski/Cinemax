
using Cinemax.Shared.Common;
using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Seats
{
    [ExcelSheet("Seats")]
    public class SeatExcelDto
    {
        [ExcelColumn("Number", Required = true)]
        public int Number { get; set; }
        [ExcelColumn("Row", Required = true)]
        public int Row { get; set; }
        [ExcelColumn("Type", Required = true)]
        public SeatType Type { get; set; }
    }
}
