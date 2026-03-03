using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GuildRomaAuth.Models;


public class GuildSettings
{
    public int Id { get; set; }
    
    public string GuildStatus { get; set; }  // status da guild 
    
    public int TaxRate { get; set; }  // taxa da guild

    public long TotalFame { get; set; }  // fama da guid

    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

}