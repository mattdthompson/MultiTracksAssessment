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
using Newtonsoft.Json;

namespace api.multitracks.com.Controllers
{
    [ApiController]
    public class ArtistController : ControllerBase
    {
        /// <summary>
        /// API endpoint/method to search artist by name.
        /// Name can be complete or partial.
        /// If name is partial it may return multiple results
        /// For example: if artistName is "Brandon" records "Brandon Lake" and "Brandon Heath" could be returned
        /// TODO: send back artists that are close matches (i.e. "Brendon" could return "Brandon Lake")
        /// </summary>
        /// <param name="artistName">string containing artist full or partial name</param>
        [HttpGet("/artist/search")]
        public IEnumerable<Artist> SearchArtist([FromQuery(Name = "artistName")] string artistName)
        {
            var sql = new SQL();

            //Create and execute query, return dataset
            string query = $"Select * from Artist where title like '%{artistName}%' ";
            var ds = sql.ExecuteDS(query);

            //Map DataSet to DTO
            IEnumerable<Artist> artists = ds.Tables[0].AsEnumerable().Select(x => new Artist()
            {
                artistID = x.Field<int>("artistID"),
                dateCreation = x.Field<DateTime>("dateCreation"),
                title = x.Field<string>("title"),
                biography = x.Field<string>("biography"),
                imageURL = x.Field<string>("imageURL"),
                heroURL = x.Field<string>("heroURL")
            });

            return artists;
        }

        /// <summary>
        /// API endpoint/method to add artist.
        /// TODO: checking the body and returning more helpful messages when data types are off
        /// TODO: check the database for a record with the same name before inserting (may need to some business rules on how that would be handled)
        /// </summary>
        /// <param name="artist">Artist DTO for MultiTracksDB</param>
        /// <returns></returns>
        [HttpPost("/artist/add")]
        public IActionResult AddArtist([FromBody] Artist artist)
        {
            //Reject if Model state is not valid
            if (!ModelState.IsValid)
            {
                return BadRequest("Artist model not properly defined");
            }
            var sql = new SQL();

            // Create Parameterized SQL statement
            string command = "INSERT INTO Artist (dateCreation, title, biography, imageURL, heroURL) VALUES (@dateCreation, @title, @biography, @imageURL, @heroURL)";

            //dateCreation parameter is not sent from source
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@dateCreation";
            param.Value = DateTime.Now;
            sql.Parameters.Add(param);

            // ARTIST TITLE
            param = new SqlParameter();
            param.ParameterName = "@title";
            param.Value = artist.title;
            sql.Parameters.Add(param);

            // ARTIST BIO
            param = new SqlParameter();
            param.ParameterName = "@biography";
            param.Value = artist.biography;
            sql.Parameters.Add(param);

            // ARTIST IMAGE
            param = new SqlParameter();
            param.ParameterName = "@imageURL";
            param.Value = artist.imageURL;
            sql.Parameters.Add(param);

            // ARTIST HERO IMAGE
            param = new SqlParameter();
            param.ParameterName = "@heroURL";
            param.Value = artist.heroURL;
            sql.Parameters.Add(param);
            var success = sql.Execute(command);
            if (success != 0)
            {
                // Fetch Artist record that was just created so caller has the correct object ID
                artist = SearchArtist(artist.title).FirstOrDefault();
                return Ok(artist);
            }
            else
            {
                return BadRequest("Unable to add artist record to database");
            }
        }
    }
}
