using SpotifyAPI.Local;
using System;
using System.Threading;

namespace SpotifyConnector
{
    public class SpotifyConnector
    {
        public SpotifyConnector()
        {
            
        }

        public void StartSpotify()
        {
            int numberOfTries = 12;
            for (int i = numberOfTries - 1; i >= 0; i--)
            {
                if (!SpotifyLocalAPI.IsSpotifyRunning())
                {
                    SpotifyLocalAPI.RunSpotify();
                    Thread.Sleep(5000);
                }
                else
                {
                    break;
                }
            }

            if (numberOfTries <= 0)
            {
                throw new Exception("Cannot start Spotify");
            }
        }

        public void StartPlaylist()
        {
            SpotifyLocalAPI spotifyLocalAPI = new SpotifyLocalAPI(50);

            if (!spotifyLocalAPI.Connect())
            {
                throw new Exception("Cannot connect to Spotify");
            }

            spotifyLocalAPI.PlayURL("https://open.spotify.com/user/doublejmusicltd/playlist/6ChXRsZP7VVQd0LpTcca6P", "");
        }
    }
}
