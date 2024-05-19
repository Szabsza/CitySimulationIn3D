using System.Globalization;
using System.Text.RegularExpressions;

namespace Project.Components.Obj;

public class Face
{
    private const int MinimumDataLength = 4;
    private const string LineClassifier = "f";

    public string UseMtl { get; set; } = null!;
    public int[] VIndexList { get; set; } = null!;
    public int[] VnIndexList { get; set; } = null!;
    public int[] VtIndexList { get; set; } = null!;

    public void LoadFromLineData(string lineDataRaw)
    {
        var lineData = lineDataRaw.Split(' ');

        if (lineData.Length < MinimumDataLength)
        {
            throw new ArgumentException("LineData length: " + lineData.Length + " should be: " + MinimumDataLength);
        }

        if (lineData[0] != LineClassifier)
        {
            throw new ArgumentException("LineClassifier mismatch: " + lineData[0] + " should be: " + LineClassifier);
        }

        var vcount = lineData.Length - 1;

        VIndexList = new int[vcount];
        VnIndexList = new int[vcount];
        VtIndexList = new int[vcount];

        var faceRegex = new Regex(@"^(\d+)(?:/(\d+)?(?:/(\d+))?)?$");

        for (var i = 1; i <= vcount; i++)
        {
            var match = faceRegex.Match(lineData[i]);
            if (!match.Success)
            {
                throw new FormatException("Face data format is incorrect: " + lineData[i]);
            }

            VIndexList[i - 1] = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

            if (match.Groups[2].Success)
            {
                VtIndexList[i - 1] = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
            }
            else
            {
                VtIndexList[i - 1] = -1;
            }

            if (match.Groups[3].Success)
            {
                VnIndexList[i - 1] = int.Parse(match.Groups[3].Value);
            }
            else
            {
                VnIndexList[i - 1] = -1;
            }
        }
    }
}