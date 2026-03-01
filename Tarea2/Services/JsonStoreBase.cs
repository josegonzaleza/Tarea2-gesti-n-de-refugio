using System.IO;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Tarea2.Services
{
    public abstract class JsonStoreBase
    {
        protected readonly JavaScriptSerializer _ser = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };

        protected string Map(string relative) => HttpContext.Current.Server.MapPath(relative);

        protected T Read<T>(string path, T fallback)
        {
            try
            {
                if (!File.Exists(path)) return fallback;
                var json = File.ReadAllText(path, Encoding.UTF8);
                var obj = _ser.Deserialize<T>(json);
                return obj == null ? fallback : obj;
            }
            catch
            {
                return fallback;
            }
        }

        protected void Write<T>(string path, T data)
        {
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var json = _ser.Serialize(data);
            var tmp = path + ".tmp";

            File.WriteAllText(tmp, json, Encoding.UTF8);
            File.Copy(tmp, path, true);
            File.Delete(tmp);
        }
    }
}