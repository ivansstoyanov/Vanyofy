using SpotifyAPI.Local;

namespace SpotifyConnector
{
    public class SpotifyConnector
    {
        public SpotifyConnector()
        {
            SpotifyLocalAPI spotifyLocalAPI = new SpotifyLocalAPI(50);

            if (!spotifyLocalAPI.Connect())
            {
                //Program.FailureHandling(inputArguments, null, true);
            }

            spotifyLocalAPI.PlayURL("https://open.spotify.com/user/doublejmusicltd/playlist/6ChXRsZP7VVQd0LpTcca6P", "");
        }
    }
}
