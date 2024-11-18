using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Indices")]
    public class Indice
    {
        [Key]
        public Guid id { get; set; }
        public long categoryid { get; set; }
        public DateTime createdatetime { get; set; }
        public long creatoruserid { get; set; }
        public decimal price { get; set; }
        public bool isvisible { get; set; }
        public int courseleveltypeId { get; set; }
        public string? coursetitle { get; set; }
        public string? excerpt { get; set; }
        public string? author { get; set; }
        public string? instrumentname { get; set; }
        public string? fundamentalheading { get; set; }
        public string? technicalheading { get; set; }
        public string? marketsessiontitle { get; set; }
        public string? marketsessionscript { get; set; }
        public string? marketsentimentstitle { get; set; }
        public string? marketsentimentsscript { get; set; }
        public string? relatedresorces { get; set; }
        public string? privatenotes { get; set; }
        public string? newstickernew { get; set; }
        public string? newstickerupdate { get; set; }
        public string? newstickerimportant { get; set; }
        public string? parentindex { get; set; }
        public string? indicesinformations_countriesrepresented { get; set; }
        public string? indicesinformations_centralbank { get; set; }
        public string? indicesinformations_nickname { get; set; }
        public string? indicesinformations_relatedconstituents { get; set; }
        public string? indicesinformations_weightageoflargestconstituent { get; set; }
        public string? indicesinformations_weightageoftop5constituents { get; set; }
        public string? indicesinformations_alltimehigh { get; set; }
        public string? indicesinformations_alltimelow { get; set; }
        public string? indicesinformations_warketcapitalization { get; set; }
        public string? indicesinformations_weightingmethodology { get; set; }
        public string? indicesinformations_yeartodatereturn { get; set; }
        public string? indicesinformations_pricetoearningratio { get; set; }
        public string? chart { get; set; }
        public string? maindescription { get; set; }
        public string? maindescription_filecontenttype { get; set; }
        public string? maindescription_filepath { get; set; }
        public string? maindescription_filename { get; set; }

        public List<Indice_FundamentalNewsSection>? fundamentalnewssectionlist { get; set; }
    }
}

