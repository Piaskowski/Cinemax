
using Cinemax.Shared.Common;

namespace Cinemax.Shared.Contracts.Scrennings
{
    [ExcelSheet("Screenings")]
    public class ScreeningExcelDto
    {
        [ExcelColumn("MovieTitle", Required = true)]
        public string MovieTitle { get; set; } = default!;
        [ExcelColumn("CinemaHallNumber", Required = true)]
        public int Number { get; set; }
        [ExcelColumn("StartTime", Required = true)]
        public DateTime StartTime { get; set; }
    }
}
