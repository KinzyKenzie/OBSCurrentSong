/// OBSCurrentSong Project by Rotinx
/// https://github.com/Rotinx/OBSCurrentSong
/// 
/// Improvements made by KinzyKenzie
/// https://github.com/KinzyKenzie/OBSCurrentSong
/// 
/// Old code sourced through dnSpy from OBSCurrentSong Release V1.28

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OBSCurrentSong
{

    public class Config
    {
        public string spacing = null;
        public string subprefix = null;
        public string separator = null;
    }

    class Program
    {
        private static Config OBConfig;

        static void Main() {

            if( !File.Exists( "config.json" ) ) {

                Console.WriteLine( "No config file found. Creating..." );

                OBConfig = new Config();
                string contents = JsonConvert.SerializeObject( OBConfig, Formatting.Indented );
                File.WriteAllText( "config.json", contents );

            } else
                OBConfig = JsonConvert.DeserializeObject<Config>( File.ReadAllText( "config.json" ) );

            if( !File.Exists( "temp" ) ) {

                Console.WriteLine( "Looks like this is your first time launching. Opening support page..." );

                File.WriteAllText( "temp", "" );
                Process.Start( "support.html" );

            } else
                OBConfig = JsonConvert.DeserializeObject<Config>( File.ReadAllText( "config.json" ) );

            Console.Title = "OBSCurrentSong";
            string text = "";
            Console.WriteLine( "Ready... and waiting!" );

            for(; ; )
            {
                try {

                    Process[] processesByName = Process.GetProcessesByName( "Spotify" );

                    if( processesByName.Length == 0 ) {
                        Console.Clear();
                        Console.WriteLine( "Please start Spotify." );
                        File.WriteAllText( "./currentsong.txt", "" );
                        File.WriteAllText( "./artist.txt", "" );
                        File.WriteAllText( "./song.txt", "" );
                    }

                    Process[] array = processesByName;

                    for( int i = 0; i < array.Length; i++ ) {

                        string mainWindowTitle = array[ i ].MainWindowTitle;

                        if( mainWindowTitle != "" && mainWindowTitle != text ) {

                            Console.Clear();
                            string[] array2 = mainWindowTitle.Split( new char[] { '-' }, 2 );

                            if( array2[ 0 ] != "Spotify" && array2[ 0 ] != "Spotify Free" && array2[ 0 ] != "Spotify Premium" ) {

                                File.WriteAllText( "./artist.txt", array2[ 0 ] );
                                File.WriteAllText( "./song.txt", array2[ 1 ].TrimStart( new char[] { ' ' } ) );

                                File.WriteAllText( "./currentsong.txt", string.Concat( new string[]
                                {
                                    OBConfig.subprefix,
                                    array2[1].TrimStart(new char[] { ' ' }),
                                    OBConfig.separator,
                                    array2[0],
                                    OBConfig.spacing
                                } ) );

                                Console.WriteLine( "Currently playing: " + mainWindowTitle );
                                Console.Title = "OBSCurrentSong | Currently playing: " + mainWindowTitle;
                                Console.WriteLine( "Prev song: " + text );
                                text = mainWindowTitle;

                            } else {

                                File.WriteAllText( "./artist.txt", "" );
                                File.WriteAllText( "./song.txt", "" );
                                File.WriteAllText( "./currentsong.txt", "" );

                                Console.WriteLine( "Currently playing: " );
                                Console.Title = "OBSCurrentSong | Currently playing: ";
                                Console.WriteLine( "Prev song: " + text );
                                text = "";
                            }
                        }
                    }

                    Thread.Sleep( 3000 );

                } catch( Exception ) { }
            }
        }
    }
}
