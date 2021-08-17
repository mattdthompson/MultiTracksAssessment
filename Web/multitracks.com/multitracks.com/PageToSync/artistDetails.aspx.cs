using DataAccess;
using System;
using System.Data;

public partial class PageToSync_artistDetails : MultitracksPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var sql = new SQL();
        try
        {
            sql.Parameters.Add("@artistId", 1);
            var data = sql.ExecuteStoredProcedureDS("GetArtistDetails");

            HeroImg.ImageUrl = (string)data.Tables[0].FirstOrNewRow().ItemArray[3];
            AvatarImg.ImageUrl = (string)data.Tables[0].FirstOrNewRow().ItemArray[2];
            ArtistBio.InnerText = (string)data.Tables[0].FirstOrNewRow().ItemArray[1];
            ArtistName.InnerText = (string)data.Tables[0].FirstOrNewRow().ItemArray[0];
            //assessmentItems.DataSource = data;
            //assessmentItems.DataBind();

            //publishDB.Visible = false;
            //items.Visible = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            //publishDB.Visible = true;
            //items.Visible = false;
        }
    }
}