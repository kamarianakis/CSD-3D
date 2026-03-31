/*This script handles the reading of teachers' information from a repository and the assigning of them
to the teachers panel*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Net;
using System.IO;
using UnityEngine.Networking; // Added for UnityWebRequest

public class TeachersCatalogue : MonoBehaviour
{
    public string teacherURL = "https://raw.githubusercontent.com/kamarianakis/CSD-3D/refs/heads/main/Excel%20Files/TeachersInfo.csv"; // GitHub CSV file URL
    public bool isLocalURL = false;

    public GameObject templateButton; // The template button to clone
    public GameObject templateButton2; // The template button to clone
    public Transform scrollContent; // The content holder for scroll view
    public Transform scrollContent2; // The content holder for scroll view
    public GameObject panelToHide; // The panel to hide
    public GameObject panelToHide2; // The panel to hide
    public GameObject player; // Player GameObject (WaypointSystem script should be attached here)
    public List<GameObject> waypointObjects; // List of GameObjects
    private List<string> Rooms = new List<string> {
        "ΑΜΦ ΣΟ","ΑΜΦΣΟ","AMF SO","AMFSO",
        "ΑΜΦ Α","ΑΜΦΑ","AMF A","AMFA",
        "Α.101","A.101","Α101","A101","Α-101","A-101",
        "Α.103","A.103","Α103","A103","Α-103","A-103",
        "Α.105","A.105","Α105","A105","Α-105","A-105",
        "Α.107","A.107","Α107","A107","Α-107","A-107",
        "Α.109","A.109","Α109","A109","Α-109","A-109",
        "Α.111","A.111","Α111","A111","Α-111","A-111",
        "Α.113","A.113","Α113","A113","Α-113","A-113",
        "Α.115","A.115","Α115","A115","Α-115","A-115",
        "Α.117","A.117","Α117","A117","Α-117","A-117",
        "Α.119","A.119","Α119","A119","Α-119","A-119",
        "Α.121","A.121","Α121","A121","Α-121","A-121",
        "Α.123","A.123","Α123","A123","Α-123","A-123",
        "Α.125","A.125","Α125","A125","Α-125","A-125",
        "Β.101","B.101","Β101","B101","Β-101","B-101",
        "Β.102","B.102","Β102","B102","Β-102","B-102",
        "Β.103","B.103","Β103","B103","Β-103","B-103",
        "Β.104","B.104","Β104","B104","Β-104","B-104",
        "Β.105","B.105","Β105","B105","Β-105","B-105",
        "Β.106","B.106","Β106","B106","Β-106","B-106",
        "Β.107","B.107","Β107","B107","Β-107","B-107",
        "Β.108","B.108","Β108","B108","Β-108","B-108",
        "Β.109","B.109","Β109","B109","Β-109","B-109",
        "Β.110","B.110","Β110","B110","Β-110","B-110",
        "Β.111","B.111","Β111","B111","Β-111","B-111",
        "Β.112","B.112","Β112","B112","Β-112","B-112",
        "Β.113","B.113","Β113","B113","Β-113","B-113",
        "Β.115","B.115","Β115","B115","Β-115","B-115",
        "Ε.101","E.101","Ε101","E101","Ε-101","E-101",
        "Ε.102","E.102","Ε102","E102","Ε-102","E-102",
        "Ε.103","E.103","Ε103","E103","Ε-103","E-103",
        "Ε.104","E.104","Ε104","E104","Ε-104","E-104",
        "Ε.105","E.105","Ε105","E105","Ε-105","E-105",
        "Ε.106","E.106","Ε106","E106","Ε-106","E-106",
        "Ε.108","E.108","Ε108","E108","Ε-108","E-108",
        "Ε.110","E.110","Ε110","E110","Ε-110","E-110",
        "Ε.112","E.112","Ε112","E112","Ε-112","E-112",
        "Η.101","H.101","Η101","H101","Η-101","H-101",
        "Η.102","H.102","Η102","H102","Η-102","H-102",
        "Η.103","H.103","Η103","H103","Η-103","H-103",
        "Η.104","H.104","Η104","H104","Η-104","H-104",
        "Η.105","H.105","Η105","H105","Η-105","H-105",
        "Η.106","H.106","Η106","H106","Η-106","H-106",
        "Η.107","H.107","Η107","H107","Η-107","H-107",
        "Η.108","H.108","Η108","H108","Η-108","H-108",
        "Η.110","H.110","Η110","H110","Η-110","H-110",
        "Η.112","H.112","Η112","H112","Η-112","H-112",
        "Η.114","H.114","Η114","H114","Η-114","H-114",
        "Ε.201","E.201","Ε201","E201","Ε-201","E-201",
        "Ε.202","E.202","Ε202","E202","Ε-202","E-202",
        "Ε.203","E.203","Ε203","E203","Ε-203","E-203",
        "Ε.204","E.204","Ε204","E204","Ε-204","E-204",
        "Ε.205","E.205","Ε205","E205","Ε-205","E-205",
        "Ε.206","E.206","Ε206","E206","Ε-206","E-206",
        "Ε.207","E.207","Ε207","E207","Ε-207","E-207",
        "Ε.208","E.208","Ε208","E208","Ε-208","E-208",
        "Ε.210","E.210","Ε210","E210","Ε-210","E-210",
        "Η.201","H.201","Η201","H201","Η-201","H-201",
        "Η.202","H.202","Η202","H202","Η-202","H-202",
        "Η.203","H.203","Η203","H203","Η-203","H-203",
        "Η.204","H.204","Η204","H204","Η-204","H-204",
        "Η.205","H.205","Η205","H205","Η-205","H-205",
        "Η.206","H.206","Η206","H206","Η-206","H-206",
        "Η.208","H.208","Η208","H208","Η-208","H-208",
        "Η.210","H.210","Η210","H210","Η-210","H-210",
        "Η.212","H.212","Η212","H212","Η-212","H-212",
        "Η.214","H.214","Η214","H214","Η-214","H-214",
        "Η.216","H.216","Η216","H216","Η-216","H-216",
        "Β.201","B.201","Β201","B201","Β-201","B-201",
        "Β.202","B.202","Β202","B202","Β-202","B-202",
        "Β.203","B.203","Β203","B203","Β-203","B-203",
        "Β.204","B.204","Β204","B204","Β-204","B-204",
        "Β.205","B.205","Β205","B205","Β-205","B-205",
        "Β.206","B.206","Β206","B206","Β-206","B-206",
        "Β.207","B.207","Β207","B207","Β-207","B-207",
        "Β.208","B.208","Β208","B208","Β-208","B-208",
        "Β.209","B.209","Β209","B209","Β-209","B-209",
        "Β.210","B.210","Β210","B210","Β-210","B-210",
        "Β.211","B.211","Β211","B211","Β-211","B-211",
        "Β.212","B.212","Β212","B212","Β-212","B-212",
        "Β.213","B.213","Β213","B213","Β-213","B-213",
        "Β.214","B.214","Β214","B214","Β-214","B-214",
        "Β.215","B.215","Β215","B215","Β-215","B-215",
        "Β.217","B.217","Β217","B217","Β-217","B-217",
        "Β.219","B.219","Β219","B219","Β-219","B-219",
        "Β.221","B.221","Β221","B221","Β-221","B-221",
        "Β.223","B.223","Β223","B223","Β-223","B-223",
        "Β.225","B.225","Β225","B225","Β-225","B-225",
        "Β.227","B.227","Β227","B227","Β-227","B-227",
        "Β.229","B.229","Β229","B229","Β-229","B-229",
        "Κ.201","K.201","Κ201","K201","Κ-201","K-201",
        "Κ.202","K.202","Κ202","K202","Κ-202","K-202",
        "Κ.203","K.203","Κ203","K203","Κ-203","K-203",
        "Κ.204","K.204","Κ204","K204","Κ-204","K-204",
        "Κ.205","K.205","Κ205","K205","Κ-205","K-205",
        "Κ.206","K.206","Κ206","K206","Κ-206","K-206",
        "Κ.207","K.207","Κ207","K207","Κ-207","K-207",
        "Κ.208","K.208","Κ208","K208","Κ-208","K-208",
        "Κ.209","K.209","Κ209","K209","Κ-209","K-209",
        "Κ.210","K.210","Κ210","K210","Κ-210","K-210",
        "Κ.211","K.211","Κ211","K211","Κ-211","K-211",
        "Κ.212","K.212","Κ212","K212","Κ-212","K-212",
        "Κ.213","K.213","Κ213","K213","Κ-213","K-213",
        "Κ.214","K.214","Κ214","K214","Κ-214","K-214",
        "Κ.215","K.215","Κ215","K215","Κ-215","K-215",
        "Κ.216","K.216","Κ216","K216","Κ-216","K-216",
        "Κ.217","K.217","Κ217","K217","Κ-217","K-217",
        "Κ.218","K.218","Κ218","K218","Κ-218","K-218",
        "Κ.219","K.219","Κ219","K219","Κ-219","K-219",
        "Κ.220","K.220","Κ220","K220","Κ-220","K-220",
        "Κ.221","K.221","Κ221","K221","Κ-221","K-221",
        "Κ.223","K.223","Κ223","K223","Κ-223","K-223",
        "Κ.225","K.225","Κ225","K225","Κ-225","K-225",
        "Κ.227","K.227","Κ227","K227","Κ-227","K-227",
        "Ε.301","E.301","Ε301","E301","Ε-301","E-301",
        "Ε.302","E.302","Ε302","E302","Ε-302","E-302",
        "Ε.303","E.303","Ε303","E303","Ε-303","E-303",
        "Ε.304","E.304","Ε304","E304","Ε-304","E-304",
        "Ε.305","E.305","Ε305","E305","Ε-305","E-305",
        "Ε.306","E.306","Ε306","E306","Ε-306","E-306",
        "Ε.307","E.307","Ε307","E307","Ε-307","E-307",
        "Ε.308","E.308","Ε308","E308","Ε-308","E-308",
        "Ε.309","E.309","Ε309","E309","Ε-309","E-309",
        "Ε.310","E.310","Ε310","E310","Ε-310","E-310",
        "Ε.311","E.311","Ε311","E311","Ε-311","E-311",
        "Ε.313","E.313","Ε313","E313","Ε-313","E-313",
        "Ε.315","E.315","Ε315","E315","Ε-315","E-315",
        "Ε.317","E.317","Ε317","E317","Ε-317","E-317",
        "Η.301","H.301","Η301","H301","Η-301","H-301",
        "Η.302","H.302","Η302","H302","Η-302","H-302",
        "Η.303","H.303","Η303","H303","Η-303","H-303",
        "Η.304","H.304","Η304","H304","Η-304","H-304",
        "Η.305","H.305","Η305","H305","Η-305","H-305",
        "Η.306","H.306","Η306","H306","Η-306","H-306",
        "Η.307","H.307","Η307","H307","Η-307","H-307",
        "Η.308","H.308","Η308","H308","Η-308","H-308",
        "Η.309","H.309","Η309","H309","Η-309","H-309",
        "Η.310","H.310","Η310","H310","Η-310","H-310",
        "Η.311","H.311","Η311","H311","Η-311","H-311",
        "Η.312","H.312","Η312","H312","Η-312","H-312",
        "Η.314","H.314","Η314","H314","Η-314","H-314",
        "Η.316","H.316","Η316","H316","Η-316","H-316",
        "Β.301","B.301","Β301","B301","Β-301","B-301",
        "Β.302","B.302","Β302","B302","Β-302","B-302",
        "Β.303","B.303","Β303","B303","Β-303","B-303",
        "Β.304","B.304","Β304","B304","Β-304","B-304",
        "Β.305","B.305","Β305","B305","Β-305","B-305",
        "Β.306","B.306","Β306","B306","Β-306","B-306",
        "Β.307","B.307","Β307","B307","Β-307","B-307",
        "Β.308","B.308","Β308","B308","Β-308","B-308",
        "Β.309","B.309","Β309","B309","Β-309","B-309",
        "Β.310","B.310","Β310","B310","Β-310","B-310",
        "Β.311","B.311","Β311","B311","Β-311","B-311",
        "Β.312","B.312","Β312","B312","Β-312","B-312",
        "Β.313","B.313","Β313","B313","Β-313","B-313",
        "Β.314","B.314","Β314","B314","Β-314","B-314",
        "Β.315","B.315","Β315","B315","Β-315","B-315",
        "Β.316","B.316","Β316","B316","Β-316","B-316",
        "Β.317","B.317","Β317","B317","Β-317","B-317",
        "Β.318","B.318","Β318","B318","Β-318","B-318",
        "Β.319","B.319","Β319","B319","Β-319","B-319",
        "Β.320","B.320","Β320","B320","Β-320","B-320",
        "Β.321","B.321","Β321","B321","Β-321","B-321",
        "Β.322","B.322","Β322","B322","Β-322","B-322",
        "Β.323","B.323","Β323","B323","Β-323","B-323",
        "Β.325","B.325","Β325","B325","Β-325","B-325",
        "Β.327","B.327","Β327","B327","Β-327","B-327",
        "Β.329","B.329","Β329","B329","Β-329","B-329",
        "Κ.301","K.301","Κ301","K301","Κ-301","K-301",
        "Κ.302","K.302","Κ302","K302","Κ-302","K-302",
        "Κ.303","K.303","Κ303","K303","Κ-303","K-303",
        "Κ.304","K.304","Κ304","K304","Κ-304","K-304",
        "Κ.305","K.305","Κ305","K305","Κ-305","K-305",
        "Κ.306","K.306","Κ306","K306","Κ-306","K-306",
        "Κ.307","K.307","Κ307","K307","Κ-307","K-307",
        "Κ.308","K.308","Κ308","K308","Κ-308","K-308",
        "Κ.309","K.309","Κ309","K309","Κ-309","K-309",
        "Κ.310","K.310","Κ310","K310","Κ-310","K-310",
        "Κ.311","K.311","Κ311","K311","Κ-311","K-311",
        "Κ.312","K.312","Κ312","K312","Κ-312","K-312",
        "Κ.313","K.313","Κ313","K313","Κ-313","K-313",
        "Κ.314","K.314","Κ314","K314","Κ-314","K-314",
        "Κ.315","K.315","Κ315","K315","Κ-315","K-315",
        "Κ.316","K.316","Κ316","K316","Κ-316","K-316",
        "Κ.317","K.317","Κ317","K317","Κ-317","K-317",
        "Κ.319","K.319","Κ319","K319","Κ-319","K-319",
        "Κ.321","K.321","Κ321","K321","Κ-321","K-321",
        "Κ.323","K.323","Κ323","K323","Κ-323","K-323",
        "Κ.325","K.325","Κ325","K325","Κ-325","K-325",
        "Κ.327","K.327","Κ327","K327","Κ-327","K-327",
        "Κ.329","K.329","Κ329","K329","Κ-329","K-329"
    };

    /*At start, a function is called to read all the data and assign it to the teacher panel*/
    void Start()
    {
        if (isLocalURL)
        {
            string csvData = LocalFileReader.LoadText(teacherURL);
            LoadFromCSV(csvData);
        }
        else
        {
            StartCoroutine(LoadCSV(teacherURL)); // Use StartCoroutine to load CSV
        }
    }

    /*The function below handles the reading of the data of from the repository*/
    IEnumerator LoadCSV(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url); // Use UnityWebRequest to download data
        yield return request.SendWebRequest(); // Wait for request completion

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load CSV: " + request.error);
            yield break;
        }

        string csvData = request.downloadHandler.text; // Get the CSV text data
        LoadFromCSV(csvData);
    }

    private void LoadFromCSV(string csvData) {
        ParseCSV(csvData);

        void ParseCSV(string csvData)
        {
            StringReader reader = new StringReader(csvData);
            List<string[]> rows = new List<string[]>();
            string line;
            int lineIndex = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineIndex++;
                if (lineIndex <= 18 || reader.Peek() == -1) continue; // Ignore first 18 lines
                if (string.IsNullOrWhiteSpace(line)) break; // Stop at empty lines

                // Use proper CSV parser instead of Split(',')
                string[] fields = ParseCsvLineSimple(line);
                if (fields.Length < 5) continue; // Ensure enough columns

                rows.Add(fields);
            }

            PopulateScrollView(rows);
        }

        string[] ParseCsvLineSimple(string line)
        {
            List<string> fields = new List<string>();
            var current = new System.Text.StringBuilder();
            bool insideQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"'); // Escaped quote
                        i++; // Skip next quote
                    }
                    else
                    {
                        insideQuotes = !insideQuotes; // Toggle quoted state
                    }
                }
                else if (c == ',' && !insideQuotes)
                {
                    fields.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            fields.Add(current.ToString());

            // Optional: Strip wrapping quotes
            for (int i = 0; i < fields.Count; i++)
            {
                string f = fields[i];
                if (f.StartsWith("\"") && f.EndsWith("\"") && f.Length >= 2)
                {
                    f = f.Substring(1, f.Length - 2);
                }
                fields[i] = f;
            }

            return fields.ToArray();
        }

        /*The function below handles the assigning of the read data from the repository to the corresponding panel*/
        void PopulateScrollView(List<string[]> data)
        {
            foreach (var fields in data)
            {
                GameObject newButton = Instantiate(templateButton, scrollContent);
                newButton.SetActive(true);

                TMP_Text roomCodeText = newButton.transform.Find("Room Code").GetComponent<TMP_Text>();
                TMP_Text professorNameText = newButton.transform.Find("Professor Name").GetComponent<TMP_Text>();
                TMP_Text specialtyText = newButton.transform.Find("Specialty").GetComponent<TMP_Text>();
                TMP_Text emailText = newButton.transform.Find("E-mail").GetComponent<TMP_Text>();
                TMP_InputField aaa = newButton.transform.Find("Aaa").GetComponent<TMP_InputField>();
                TMP_Text websiteText = newButton.transform.Find("Website").GetComponent<TMP_Text>();

                if (roomCodeText)
                    roomCodeText.text = fields[0];
                if (professorNameText) professorNameText.text = fields[1];
                if (specialtyText) specialtyText.text = fields[2];
                if (emailText)
                {
                    if (fields[3] != "")
                    {
                        emailText.text = HighlightAndLink(fields[3], fields[3]);
                        aaa.text = emailText.text;
                        TMPLinkHandler textEventHandler = emailText.gameObject.AddComponent<TMPLinkHandler>();
                    }
                    else
                    {
                        emailText.text = fields[3]; // Regular text
                    }
                }
                if (websiteText)
                {
                    if (fields[4] != "")
                    {
                        websiteText.text = HighlightAndLink(fields[4], fields[4]);
                        TMPLinkHandler textEventHandler = websiteText.gameObject.AddComponent<TMPLinkHandler>();
                    }
                    else
                    {
                        websiteText.text = fields[4]; // Regular text
                    }
                }
            }
        }
        templateButton.SetActive(false);
        panelToHide.SetActive(false);
    }

    /*The function below takes text and colors, gives it italic, underlines it and gives it a link(click) functionality*/
    string HighlightAndLink(string fullText, string keyword)
    {
        return fullText.Replace(keyword, $"<color=#93DDF5><i><u><link={keyword}>{keyword}</link></u></i></color>");
    }

    public void Targetsetting()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        TMP_Text roomCodeText = clickedButton.transform.Find("Room Code").GetComponent<TMP_Text>();
        if (RoomFinder(roomCodeText.text) != "")
        {
            TMP_TextEventHandler textEventHandler = roomCodeText.gameObject.AddComponent<TMP_TextEventHandler>();
            textEventHandler.Setup(player, waypointObjects, panelToHide, roomCodeText.text, 1);
        }
    }

    /*The function below takes some text and finds in it the needed text that needs to be highlighted, one of the options inside the 
    Rooms array*/
    private string RoomFinder(string givenString)
    {
        // Check which keywords are in the given string
        foreach (string keyword in Rooms)
        {
            if (givenString.Contains(keyword))
            {
                return keyword;
            }
        }
        return "";
    }
}
