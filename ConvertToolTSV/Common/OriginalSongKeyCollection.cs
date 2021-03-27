using System.Collections.Generic;
using System.Linq;

namespace PLTextToolTSV.Common
{
    public class OriginalSongKeyCollection
    {
        private IList<SongKeyEntity> songList;

        public OriginalSongKeyCollection()
        {
            songList = new List<SongKeyEntity>();
            songList.Add(new SongKeyEntity { SongKey = 0xfa, SongInfo = "[♭6][DBN値:-6]" });
            songList.Add(new SongKeyEntity { SongKey = 0xfb, SongInfo = "[♭5][DBN値:-5]" });
            songList.Add(new SongKeyEntity { SongKey = 0xfc, SongInfo = "[♭4][DBN値:-4]" });
            songList.Add(new SongKeyEntity { SongKey = 0xfd, SongInfo = "[♭3][DBN値:-3]" });
            songList.Add(new SongKeyEntity { SongKey = 0xfe, SongInfo = "[♭2][DBN値:-2]" });
            songList.Add(new SongKeyEntity { SongKey = 0xff, SongInfo = "[♭1][DBN値:-1]" });
            songList.Add(new SongKeyEntity { SongKey = 0x00, SongInfo = "[原曲と同じキー][DBN値:0]" });
            songList.Add(new SongKeyEntity { SongKey = 0x01, SongInfo = "[＃1][DBN値:+1]" });
            songList.Add(new SongKeyEntity { SongKey = 0x02, SongInfo = "[＃2][DBN値:+2]" });
            songList.Add(new SongKeyEntity { SongKey = 0x03, SongInfo = "[＃3][DBN値:+3]" });
            songList.Add(new SongKeyEntity { SongKey = 0x04, SongInfo = "[＃4][DBN値:+4]" });
            songList.Add(new SongKeyEntity { SongKey = 0x05, SongInfo = "[＃5][DBN値:+5]" });
            songList.Add(new SongKeyEntity { SongKey = 0x06, SongInfo = "[＃6][DBN値:+6]" });
            songList.Add(new SongKeyEntity { SongKey = 0x7f, SongInfo = "[キーなし][DBN値:Ｎ]" });            
        }

        public string GetSongInfo(byte songKey)
        {
            var exist = songList.Where(r => r.SongKey == songKey).FirstOrDefault();
            if (exist != null)
                return exist.SongInfo;
            // Default value
            return "[異常データ]";
        }
    }

    public class SongKeyEntity
    {
        public byte SongKey { get; set; }
        public string SongInfo { get; set; }
        public string Note { get; set; }
    }

}
