using api.multitracks.com.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace api.multitracks.com.Controllers
{
    [ApiController]
    public class ArtistController : ControllerBase
    {
        [HttpGet("/artist/search")]
        public IEnumerable<Artist> SearchArtist([FromQuery(Name = "artistName")] string artistName)
        {
            var sql = new SQL();
            string command = $"Select * from Artist where title like '%{artistName}%' ";
            var ds = sql.ExecuteDS(command);

            List<Artist> artists = ds.Tables[0].AsEnumerable().Select(x => new Artist()
            {
                artistID = x.Field<int>("artistID"),
                dateCreation = x.Field<DateTime>("dateCreation"),
                title = x.Field<string>("title"),
                biography = x.Field<string>("biography"),
                imageURL = x.Field<string>("imageURL"),
                heroURL = x.Field<string>("heroURL")

            }).ToList();
            return artists;
        }

        [HttpPost("/artist/add")]
        public IActionResult AddArtist([FromBody] Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Artist model not properly defined");
            }
            var sql = new SQL();
            string command = "INSERT INTO Artist (dateCreation, title, biography, imageURL, heroURL) VALUES (@dateCreation, @title, @biography, @imageURL, @heroURL)";
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@dateCreation";
            param.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            sql.Parameters.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@title";
            param.Value = artist.title;
            sql.Parameters.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@biography";
            param.Value = artist.biography;
            sql.Parameters.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@imageURL";
            param.Value = artist.imageURL;
            sql.Parameters.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@heroURL";
            param.Value = artist.heroURL;
            sql.Parameters.Add(param);
            sql.Execute(command);

            return Ok(artist);
        }
    }
}
