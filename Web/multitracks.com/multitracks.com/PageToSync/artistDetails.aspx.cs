using DataAccess;
using System;
using System.Data;
using System.Linq;

public partial class PageToSync_artistDetails : MultitracksPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int artistId;
        var queryArtistId = Request.QueryString["artistID"];
        Int32.TryParse(queryArtistId, out artistId);
        if (artistId == 0) artistId = 1;
        var sql = new SQL();
        try
        {
            sql.Parameters.Add("@artistId", artistId);
            var data = sql.ExecuteStoredProcedureDT("GetArtistDetails");
            SongDisplay.DataSource = data;
            SongDisplay.DataBind();
            data.Select();
            var distinctAlbums = data.AsEnumerable().Select(x => new
            {
                AlbumTitle=x.Field<string>("AlbumTitle"),
                AlbumImg = x.Field<string>("AlbumImg"),
                ArtistTitle = x.Field<string>("ArtistTitle"),
            }).Distinct().ToList();
            AlbumDisplay.DataSource = distinctAlbums;
            AlbumDisplay.DataBind();
            HeroImg.ImageUrl = (string)data.Rows[0].ItemArray[3];
            AvatarImg.ImageUrl = (string)data.Rows[0].ItemArray[2];
            ArtistBio.InnerText = (string)data.Rows[0].ItemArray[1];
            ArtistName.InnerText = (string)data.Rows[0].ItemArray[0];
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    protected string TimeSignature(string timeSignature)
    {
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