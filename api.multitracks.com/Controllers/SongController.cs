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
        /// <summary>
        /// API endpoint/method to get list of songs for artist.
        /// </summary>
        /// <param name="pageCount">number of records requsted per page</param>
        /// <param name="pageNumber">page of records requested</param>
        /// <returns></returns>
        [HttpGet("/song/list")]
        public IEnumerable<Song> GetSongList([FromQuery(Name = "pageCount")] int pageCount, [FromQuery(Name = "pageNumber")] int pageNumber)
        {
            var sql = new SQL();

            //Create query for songs starting with the newest
            //Pagination is built into the SQL statement
            string query = $"Select * from Song ORDER BY dateCreation desc OFFSET {pageCount * (pageNumber-1)} ROWS FETCH NEXT {pageCount} ROWS ONLY";
            var ds = sql.ExecuteDS(query);

            //Map DataSet to DTO
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
