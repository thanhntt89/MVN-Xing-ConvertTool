using System.Collections.Generic;
using System.Linq;

namespace PLTextToolTXT.Common
{
    public class FontCodeCollection
    {
        IList<FontEntity> FontList;
        public FontCodeCollection()
        {
            FontList = new List<FontEntity>();
            FontList.Add(new FontEntity() { Id = 0, FontCode = "[日本語]" });
            FontList.Add(new FontEntity() { Id = 1, FontCode = "[ハングル]" });
            FontList.Add(new FontEntity() { Id = 2, FontCode = "[中国語]" });
            FontList.Add(new FontEntity() { Id = 3, FontCode = "[日本語明朝32ドット(PLE-PV用)]" });
            FontList.Add(new FontEntity() { Id = 4, FontCode = "[日本語ゴシック32ドット(PLC-PV用)]" });
            FontList.Add(new FontEntity() { Id = 5, FontCode = "[日本語ゴシック24ドット]" });
            FontList.Add(new FontEntity() { Id = 6, FontCode = "[日本語明朝32ドット(モード表示用)]" });
            FontList.Add(new FontEntity() { Id = 7, FontCode = "[韓国語32ドット(モード表示用)]" });
            FontList.Add(new FontEntity() { Id = 8, FontCode = "[中国語32ドット(モード表示用)]" });
        }

        public string GetFontCodeById(int id)
        {
            var exist = FontList.Where(r => r.Id == id).FirstOrDefault();
            if (exist != null)
            {
                return exist.FontCode;
            }
            return "[異常データ]";
        }
    }

    public class FontEntity
    {
        public int Id { get; set; }
        public string FontCode { get; set; }
    }
}
