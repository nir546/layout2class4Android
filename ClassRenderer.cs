using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Layout2Class
{
    public static class ClassRenderer
    {
        public static void CreateLayoutClasses(string package, DirectoryInfo layoutsfolder, DirectoryInfo outputFolder)
        {
            foreach (var file in layoutsfolder.GetFiles())
            {
                var lines = File.ReadAllLines(file.FullName);
                var ids = new Dictionary<string, string>();
                const string idIndicator = "android:id=\"";
                var controlType = "";
                foreach (var line in lines)
                {
                    if (line.Trim().StartsWith("<"))
                    {
                        controlType = line.Trim().Split(new[] { "<", " " }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }

                    var index = line.IndexOf(idIndicator);
                    if (index >= 0)
                    {
                        var id = line.Substring(index + idIndicator.Length, line.IndexOf("\"", index + idIndicator.Length + 2) - (index + idIndicator.Length));
                        ids.Add(id.Replace("@+id/", ""), controlType);
                    }
                }

                if (ids.Count > 0)
                {
                    var newFile = new FileInfo(Path.Combine(outputFolder.FullName, file.Name.Replace(".xml", ".java")));

                    var sb = new StringBuilder();
                    var className = file.Name.Replace(".xml", "");

                    sb.AppendLine("package " + package + ";");

                    sb.AppendLine("import android.view.View;");
                    sb.AppendLine("import android.app.Activity;");

                    var types = ids.Values.Distinct();
                    foreach (var widgetType in types)
                    {
                        sb.AppendLine("import android.widget." + widgetType + ";");
                    }

                    sb.AppendLine();

                    sb.AppendLine("public class " + className + " {");
                    sb.AppendLine("View view;");
                    sb.AppendLine("public " + className + "(View view) {");
                    sb.AppendLine("this.view = view;");
                    sb.AppendLine("}");
                    sb.AppendLine("");

                    sb.AppendLine("Activity activity;");
                    sb.AppendLine("public " + className + "(Activity activity) {");
                    sb.AppendLine("this.activity = activity;");
                    sb.AppendLine("}");
                    sb.AppendLine("");

                    foreach (var id in ids.Keys)
                    {
                        sb.AppendLine("public " + ids[id] + " get" + id[0].ToString().ToUpper() + id.Substring(1) + "() {");
                        sb.AppendLine("if (view != null) {");
                        sb.AppendLine("return (" + ids[id] + ") view.findViewById(R.id." + id + ");");
                        sb.AppendLine("} else {");
                        sb.AppendLine("return (" + ids[id] + ") activity.findViewById(R.id." + id + ");");
                        sb.AppendLine("}");
                        sb.AppendLine("}");

                        sb.AppendLine("");
                    }

                    sb.AppendLine("}");
                    File.WriteAllText(newFile.FullName, sb.ToString());
                }
            }
        }
    }
}
