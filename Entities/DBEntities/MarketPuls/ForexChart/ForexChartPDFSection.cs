using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_ForexCharts_PDFSections")]
    public class ForexChartPDFSection
    {
        [Key]
        public Guid id { get; set; }
        public Guid forexchartid { get; set; }
        public string? pdftitle { get; set; }
        public string? pdfshortcodeid { get; set; }
        public string? author { get; set; }
        public string? shortdescription { get; set; }
        public virtual ForexChart forexchart { get; set; }
    }
}
