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

        public void StartPlaylist(string url)
        {
            int numberOfTries = 12;
            for (int i = numberOfTries - 1; i >= 0; i--)
            {
                try
                {
                    SpotifyLocalAPI spotifyLocalAPI = new SpotifyLocalAPI(50);

                    if (!spotifyLocalAPI.Connect())
                    {
                        Thread.Sleep(5000);
                        continue;
                    }

                    if (!spotifyLocalAPI.GetStatus().Playing)
                    {
                        spotifyLocalAPI.PlayURL(url, "");
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            if (numberOfTries <= 0)
            {
                throw new Exception("Cannot connect to Spotify");
            }
        }
    }
}
