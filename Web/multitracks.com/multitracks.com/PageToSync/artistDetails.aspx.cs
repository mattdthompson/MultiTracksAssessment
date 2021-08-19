using DataAccess;
using System;
using System.Data;
using System.Linq;

public partial class PageToSync_artistDetails : MultitracksPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int artistId;
        // Get Artist ID from query string and parse out the int value
        var queryArtistId = Request.QueryString["artistID"];
        Int32.TryParse(queryArtistId, out artistId);
        // If artist is not found default to 1 to prevent error
        // TODO: Allow business logic to determine what should happen when no artistId is found in query string
        if (artistId == 0) artistId = 1;
        // init sql instance
        var sql = new SQL();
        try
        {
            // add artistId param and run stored procedure
            sql.Parameters.Add("@artistId", artistId);
            var data = sql.ExecuteStoredProcedureDT("GetArtistDetails");
            // Bind DataTable to display song info
            SongDisplay.DataSource = data;
            SongDisplay.DataBind();
            // Parse out distinct album objects and bind to display song info
            var distinctAlbums = data.AsEnumerable().Select(x => new
            {
                AlbumTitle=x.Field<string>("AlbumTitle"),
                AlbumImg = x.Field<string>("AlbumImg"),
                ArtistTitle = x.Field<string>("ArtistTitle"),
            }).Distinct().ToList();
            AlbumDisplay.DataSource = distinctAlbums;
            AlbumDisplay.DataBind();
            //Get Artist Info
            //Because all records are for the same artist we can take the first row
            ArtistName.InnerText = (string)data.Rows[0].ItemArray[0];
            ArtistBio.InnerText = (string)data.Rows[0].ItemArray[1];
            AvatarImg.ImageUrl = (string)data.Rows[0].ItemArray[2];
            HeroImg.ImageUrl = (string)data.Rows[0].ItemArray[3];
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    protected string TimeSignature(string timeSignature)
    {
        //QUESTION: Time signatures in the database didn't match actual
        // Is there another table that should have been used to map these or
        // is this switch statement a suitable alternative
        switch (timeSignature)
        {
            case "3":
                return "4/4";
            case "13":
                return "6/8";
            case "18":
                return "12/8";
            default:
                return "4/4";
        }
    }
}