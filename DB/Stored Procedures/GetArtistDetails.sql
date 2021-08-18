CREATE PROCEDURE [dbo].[GetArtistDetails]
	@artistId int = 0
AS
BEGIN

	SELECT
		art.title			ArtistTitle
		, art.biography		ArtistBiography
		, art.imageURL		ArtistImage
		, art.heroURL		ArtistHero
		, alb.title			AlbumTitle
		, alb.year			AlbumYear
		, alb.imageURL		AlbumImg
		, s.title			SongTitle
		, s.bpm				SongBPM
		, s.timeSignature	SongTimeSignature
		, s.multitracks		SongMultiTracks
		, s.customMix		SongCustomMix
		, s.chart			SongChart
		, s.rehearsalMix	SongRehearsalMix
		, s.patches			SongPatches
		, s.songSpecificPatches	SongSpecificPatches
		, s.proPresenter	SongProPresenter
	FROM Artist art
	LEFT OUTER JOIN Album alb ON alb.artistID = art.artistID
	LEFT OUTER JOIN Song s ON s.albumID = alb.albumID
	WHERE
		art.artistID = @artistId

END
