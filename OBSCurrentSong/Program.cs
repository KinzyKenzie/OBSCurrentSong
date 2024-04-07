/// OBSCurrentSong Project by Rotinx (2019 version)
/// https://github.com/Rotinx/OBSCurrentSong
/// 
/// Improvements made by KinzyKenzie (2022 fork)
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
        public int waittime = 2000;
        public string prefix = "\"";
        public string separator = "\", by ";
        public string postfix = null;
    }

    class Program
    {
        enum ProcessState
        {
            Initial, Waiting, Launched, Listening
        }

        private static ProcessState currentState = ProcessState.Initial;
        private static Config Config;
        private static string FiledumpConfig, FilecheckConfig;

        static void Main() {

            Console.Title = "OBSCurrentSong";

            if( !Directory.Exists( "text" ) )
                Directory.CreateDirectory( "text" );

            Config = ReadConfigJson();

            Console.WriteLine( "Ready... and waiting!\n" );

            string text = "",
                   fullName = "",
                   prevName = "",
                   artistName, songName;

            while( true ) {

                Process[] processesByName = Process.GetProcessesByName( "Spotify" );

                if( processesByName.Length == 0 && currentState != ProcessState.Waiting ) {

                    if( currentState != ProcessState.Initial )
                        Console.Clear();

                    Console.WriteLine( "Please start Spotify." );

                    File.WriteAllText( "./text/artist.txt", "" );
                    File.WriteAllText( "./text/currentsong.txt", "" );
                    File.WriteAllText( "./text/previoussong.txt", "" );
                    File.WriteAllText( "./text/song.txt", "" );

                    currentState = ProcessState.Waiting;
                }

                for( int i = 0; i < processesByName.Length; i++ ) {

                    string mainWindowTitle = processesByName[ i ].MainWindowTitle;

                    if( mainWindowTitle != "" && mainWindowTitle != text ) {

                        if( mainWindowTitle != "Spotify" && mainWindowTitle != "Spotify Free" && mainWindowTitle != "Spotify Premium" ) {

                            Console.Clear();

                            FilecheckConfig = File.ReadAllText( "./config.json" );

                            if( FiledumpConfig != FilecheckConfig )
                                Config = ReadConfigJson();

                            artistName = mainWindowTitle.Substring( 0, mainWindowTitle.IndexOf( " - " ) );
                            songName = mainWindowTitle.Substring( 3 + artistName.Length );
                            string newName =
                                Config.prefix +
                                songName +
                                Config.separator +
                                artistName +
                                Config.postfix;

                            // If song was paused and resumed, don't delete relevant info
                            if( newName != fullName ) {
                                prevName = fullName;
                                fullName = newName;
                            }

                            File.WriteAllText( "./text/artist.txt", artistName );
                            File.WriteAllText( "./text/currentsong.txt", fullName );
                            File.WriteAllText( "./text/previoussong.txt", prevName );
                            File.WriteAllText( "./text/song.txt", songName );

                            Console.Write( "Artist:\nSong:" );

                            Console.SetCursorPosition( 10, 0 );
                            Console.Write( artistName );

                            Console.SetCursorPosition( 10, 1 );
                            Console.Write( songName );

                            Console.WriteLine( $"\n\nOutput:\n{fullName}" );
                            Console.WriteLine( $"\nPrev song:\n{prevName}" );
                            text = mainWindowTitle;

                            currentState = ProcessState.Listening;

                        } else if( currentState != ProcessState.Launched ) {

                            Console.Clear();

                            File.WriteAllText( "./text/artist.txt", "" );
                            File.WriteAllText( "./text/song.txt", "" );
                            File.WriteAllText( "./text/currentsong.txt", "" );

                            Console.WriteLine( "No playback detected." );
                            Console.WriteLine( $"\nPrev song: {fullName}" );
                            text = "";

                            currentState = ProcessState.Launched;
                        }
                    }
                }

                Thread.Sleep( Config.waittime );
            }
        }

        private static Config ReadConfigJson() {
            Config output;

            if( !File.Exists( "./config.json" ) ) {
                Console.WriteLine( "No config file found. Creating...\n" );

                output = new Config();
                File.WriteAllText( "./config.json", JsonConvert.SerializeObject( output, Formatting.Indented ) );

                Thread.Sleep( 200 );

            } else {
                try {
                    output = JsonConvert.DeserializeObject<Config>( File.ReadAllText( "./config.json" ) );

                } catch( Exception ) {
                    output = new Config();

                    Console.WriteLine( "Problem when reading from \"config.json\". File will be recreated." );
                    File.WriteAllText( "./config.json", JsonConvert.SerializeObject( output, Formatting.Indented ) );

                    Thread.Sleep( 200 );

                    Console.WriteLine( "\nPress any key to continue . . ." );
                    Console.Read();
                }
            }

            FiledumpConfig = File.ReadAllText( "./config.json" );
            return output;
        }
    }
}
