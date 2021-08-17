CREATE PROCEDURE [dbo].[GetArtistDetails]
	@artistId int = 0
AS
BEGIN

	SELECT
		art.title			ArtistTitle
		, art.biography		ArtistBiography
		, art.imageURL		ArtistImage
		, art.heroURL		ArtistHero
	FROM Artist art
	LEFT OUTER JOIN Album alb ON alb.artistID = art.artistID
	LEFT OUTER JOIN Song s ON s.albumID = alb.albumID
	WHERE
		art.artistID = @artistId

END
