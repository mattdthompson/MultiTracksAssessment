using api.multitracks.com.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace api.multitracks.com.Controllers
{
    [ApiController]
    public class SongController : ControllerBase
    {
        [HttpGet("/song/list")]
        public IEnumerable<Song> GetSongList([FromQuery(Name = "pageCount")] int pageCount, [FromQuery(Name = "pageNumber")] int pageNumber)
        {
            var sql = new SQL();
            string command = $"Select * from Song ORDER BY dateCreation desc OFFSET {pageCount * (pageNumber-1)} ROWS FETCH NEXT {pageCount} ROWS ONLY";
            var ds = sql.ExecuteDS(command);

            List<Song> songs = ds.Tables[0].AsEnumerable().Select(x => new Song()
            {
                songID = x.Field<int>("songID"),
                dateCreation = x.Field<DateTime>("dateCreation"),
                albumID = x.Field<int>("albumID"),
                artistID = x.Field<int>("artistID"),
                title = x.Field<string>("title"),
                bpm = x.Field<decimal>("bpm"),
                timeSignature = x.Field<string>("timeSignature"),
                multitracks = x.Field<bool>("multitracks"),
                customMix = x.Field<bool>("customMix"),
                chart = x.Field<bool>("chart"),
                rehearsalMix = x.Field<bool>("rehearsalMix"),
                songSpecificPatches = x.Field<bool>("songSpecificPatches"),
                proPresenter = x.Field<bool>("proPresenter")
            }).ToList();
            return songs;
        }
    }
}
