using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblSignals")]
public class Signal
{
    [Key]
    public Guid Id { get; set; }
    public Guid signalchannelId { get; set; }
    public long creatoruserId { get; set; }
    public DateTime createdatetime { get; set; }
    public int? positiontypeId { get; set; }
    public int? timeframe_1min { get; set; }
    public bool? timeframe_5min { get; set; }
    public bool? timeframe_15min { get; set; }
    public bool? timeframe_30min { get; set; }
    public bool? timeframe_1houre { get; set; }
    public bool? timeframe_4houre { get; set; }
    public bool? timeframe_8houre { get; set; }
    public bool? timeframe_1day { get; set; }
    public int? timeframe_1week { get; set; }
    public int? timeframe_1month { get; set; }
    public int? analysistypeId { get; set; }
    public int? marketsycleid { get; set; }
    public int? instrumenttypeid { get; set; }
    public int? entrypointtypeId { get; set; }
    public string? entrypointtypename { get; set; }
    public decimal? entrypointtypevalue { get; set; }
    public decimal? sl { get; set; }
    public decimal? tp1 { get; set; }
    public decimal? tp2 { get; set; }
    public decimal? tp3 { get; set; }
    public string? resistance3 { get; set; }
    public string? resistance2 { get; set; }
    public string? resistance1 { get; set; }
    public string? entrypoint { get; set; }
    public string? support1 { get; set; }
    public string? support2 { get; set; }
    public string? support3 { get; set; }
    public string? description { get; set; }

    public virtual AnalysisType analysistype { get; set; }
    public virtual InstrumentType instrumenttype { get; set; }
    public virtual MarketCycle marketsycle { get; set; }
    public virtual PositionType positiontype { get; set; }
    public virtual EntryPointType entrypointtype { get; set; }
    public virtual User creatoruser { get; set; }
    
}


