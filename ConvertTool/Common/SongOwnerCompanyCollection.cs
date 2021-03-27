using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLTextToolTXT.Common
{
    public class SongOwnerCompanyCollection
    {
        private List<SongEntity> songList;
        public SongOwnerCompanyCollection()
        {
            songList = new List<SongEntity>();

            songList.Add(new SongEntity { SongCode = "00", SongType = "[通常曲（管理曲でない）]" });
            songList.Add(new SongEntity { SongCode = "01", SongType = "[ビクター（使用していない）]" });
            songList.Add(new SongEntity { SongCode = "02", SongType = "[コロムビア・古賀政夫]" });
            songList.Add(new SongEntity { SongCode = "03", SongType = "[クラウン]" });
            songList.Add(new SongEntity { SongCode = "04", SongType = "[テイチク（使用していない）]" });
            songList.Add(new SongEntity { SongCode = "05", SongType = "[キング（使用していない）]" });
            songList.Add(new SongEntity { SongCode = "06", SongType = "[東芝EMI（使用していない）]" });
            songList.Add(new SongEntity { SongCode = "07", SongType = "[徳間JAPAN（使用していない）]" });
            songList.Add(new SongEntity { SongCode = "08", SongType = "[ポリドール（使用していない）]" });
            songList.Add(new SongEntity { SongCode = "09", SongType = "[その他（使用していない]" });
            songList.Add(new SongEntity { SongCode = "10", SongType = "[ビクター]" });
            songList.Add(new SongEntity { SongCode = "11", SongType = "[コロムビア]" });
        }

        public string GetSongType(string songCode)
        {
            var val = songList.Where(r => r.SongCode.Equals(songCode)).FirstOrDefault();
            if (val == null)
                return string.Empty;
            return val.SongType;
        }
    }


    public class SongEntity
    {
        public string SongCode { get; set; }
        public string SongType { get; set; }
    }
}
