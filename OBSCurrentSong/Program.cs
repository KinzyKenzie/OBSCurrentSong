/// OBSCurrentSong Project by Rotinx
/// https://github.com/Rotinx/OBSCurrentSong
/// 
/// Improvements made by KinzyKenzie
/// https://github.com/KinzyKenzie/OBSCurrentSong
/// 
/// Old code sourced through dnSpy from OBSCurrentSong Release V1.28

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace OBSCurrentSong
{

    public class Config
    {
        public int waittime = 3000;
        public string prefix = null;
        public string separator = " by ";
        public string postfix = null;
    }

    class Program
    {
        private static bool running = true;
        private static Config OBConfig;

        static void Main() {

            Console.Title = "OBSCurrentSong";

            if( !File.Exists( "config.json" ) ) {

                Console.WriteLine( "No config file found. Creating..." );

                OBConfig = new Config();
                File.WriteAllText( "config.json", JsonConvert.SerializeObject( OBConfig, Formatting.Indented ) );

            } else
                OBConfig = ReadConfigJson();

            if( !File.Exists( "temp" ) ) {

                Console.WriteLine( "Looks like this is your first time launching. Opening support page..." );

                File.WriteAllText( "temp", "" );
                Process.Start( "support.html" );

            }

            // Pausing so the user can read about whatever just happened.
            if( !running ) {
                Console.WriteLine( "\nPress any key to continue . . ." );
                Console.Read();

                running = true;
            }

            Console.Clear();
            Console.WriteLine( "Ready... and waiting!\n" );

            string text = "",
                artistName, songName, fullName;

            while( true ) {

                Process[] processesByName = Process.GetProcessesByName( "Spotify" );

                if( processesByName.Length == 0 ) {
                    Console.Clear();
                    Console.WriteLine( "Please start Spotify." );

                    File.WriteAllText( "./currentsong.txt", "" );
                    File.WriteAllText( "./artist.txt", "" );
                    File.WriteAllText( "./song.txt", "" );
                }

                for( int i = 0; i < processesByName.Length; i++ ) {

                    string mainWindowTitle = processesByName[ i ].MainWindowTitle;

                    if( mainWindowTitle != "" && mainWindowTitle != text ) {

                        Console.Clear();

                        if( mainWindowTitle != "Spotify" && mainWindowTitle != "Spotify Free" && mainWindowTitle != "Spotify Premium" ) {

                            artistName = mainWindowTitle.Substring( 0, mainWindowTitle.IndexOf( " - " ) );
                            songName = mainWindowTitle.Substring( 3 + artistName.Length );
                            fullName =
                                OBConfig.prefix +
                                songName +
                                OBConfig.separator +
                                artistName +
                                OBConfig.postfix;

                            File.WriteAllText( "./artist.txt", artistName );
                            File.WriteAllText( "./song.txt", songName );
                            File.WriteAllText( "./currentsong.txt", fullName );

                            Console.Write( "Artist:\nSong:" );

                            Console.SetCursorPosition( 10, 0 );
                            Console.Write( artistName );

                            Console.SetCursorPosition( 10, 1 );
                            Console.Write( songName );

                            Console.WriteLine( $"\n\nOutput:\n{fullName}" );
                            Console.WriteLine( $"\nPrev song:\n{text}" );
                            text = mainWindowTitle;

                        } else {

                            File.WriteAllText( "./artist.txt", "" );
                            File.WriteAllText( "./song.txt", "" );
                            File.WriteAllText( "./currentsong.txt", "" );

                            Console.WriteLine( "No playback detected." );
                            Console.WriteLine( $"\nPrev song: {text}" );
                            text = "";
                        }
                    }
                }

                Thread.Sleep( OBConfig.waittime );
            }
        }

        private static Config ReadConfigJson() {
            Config output;

            try {
                output = JsonConvert.DeserializeObject<Config>( File.ReadAllText( "config.json" ) );

            } catch( Exception ) {
                output = new Config();

                Console.WriteLine( "Problem when reading from \"config.json\". File will be recreated." );
                File.WriteAllText( "config.json", JsonConvert.SerializeObject( output, Formatting.Indented ) );

                running = false;
            }

            return output;
        }

    }
}
