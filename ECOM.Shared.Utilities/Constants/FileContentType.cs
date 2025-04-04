using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOM.Shared.Utilities.Constants
{
	public static class FileContentType
	{
		// Documents
		public const string Pdf = "application/pdf";
		public const string Doc = "application/msword";
		public const string Docx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
		public const string Xls = "application/vnd.ms-excel";
		public const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
		public const string Ppt = "application/vnd.ms-powerpoint";
		public const string Pptx = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
		public const string Txt = "text/plain";
		public const string Csv = "text/csv";
		public const string Json = "application/json";
		public const string Xml = "application/xml";

		// Images
		public const string Jpeg = "image/jpeg";
		public const string Png = "image/png";
		public const string Gif = "image/gif";
		public const string Bmp = "image/bmp";
		public const string Webp = "image/webp";
		public const string Svg = "image/svg+xml";

		// Videos
		public const string Mp4 = "video/mp4";
		public const string Avi = "video/x-msvideo";
		public const string Mov = "video/quicktime";
		public const string Wmv = "video/x-ms-wmv";
		public const string Flv = "video/x-flv";
		public const string Mkv = "video/x-matroska";
		public const string Webm = "video/webm";

		// Audio
		public const string Mp3 = "audio/mpeg";
		public const string Wav = "audio/wav";
		public const string Ogg = "audio/ogg";
		public const string M4a = "audio/mp4";
		public const string Aac = "audio/aac";

		// Compressed Files
		public const string Zip = "application/zip";
		public const string Rar = "application/x-rar-compressed";
		public const string Tar = "application/x-tar";
		public const string Gz = "application/gzip";
		public const string SevenZip = "application/x-7z-compressed";

		// Web Files
		public const string Html = "text/html";
		public const string Css = "text/css";
		public const string Js = "application/javascript";

		// Fonts
		public const string Woff = "font/woff";
		public const string Woff2 = "font/woff2";
		public const string Ttf = "font/ttf";
		public const string Otf = "font/otf";

		// Other
		public const string Exe = "application/octet-stream";
		public const string Apk = "application/vnd.android.package-archive";
		public const string Iso = "application/x-iso9660-image";
	}
}
