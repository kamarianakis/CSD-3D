using System.Collections.Generic;
using UnityEngine;

public class RoomFinder: MonoBehaviour
{
    public static GameObject FindRoom(string word, List<GameObject> waypoints)
    {
        return waypoints.Find(obj => obj.name == FindInGameName(word));
    }

    // returns null if the word doesn't match one of rooms name
    // missing áìöéèÝáôñá
    // missing H109, H109b
    // missing add H210b
    public static string FindInGameName(string word)
    {
        string inGameName = null;

        //Auditorium SO
        if (word == "ÁÌÖ ÓÏ" || word == "ÁÌÖÓÏ" || word == "AMF SO" || word == "AMFSO")
        {
            inGameName = "GameObject - AMFSO";
        }

        //Auditorium A
        if (word == "ÁÌÖ Á" || word == "ÁÌÖÁ" || word == "AMF A" || word == "AMFA")
        {
            inGameName = "GameObject - AMFA";
        }

        //A101
        if (word == "Á.101" || word == "A.101" || word == "Á101" || word == "A101" || word == "Á-101" || word == "A-101")
        {
            inGameName = "GameObject - A101";
        }

        //A103
        if (word == "Á.103" || word == "A.103" || word == "Á103" || word == "A103" || word == "Á-103" || word == "A-103")
        {
            inGameName = "GameObject - A103";
        }

        //A105
        if (word == "Á.105" || word == "A.105" || word == "Á105" || word == "A105" || word == "Á-105" || word == "A-105")
        {
            inGameName = "GameObject - A105";
        }

        //A107
        if (word == "Á.107" || word == "A.107" || word == "Á107" || word == "A107" || word == "Á-107" || word == "A-107")
        {
            inGameName = "GameObject - A107";
        }

        //A109
        if (word == "Á.109" || word == "A.109" || word == "Á109" || word == "A109" || word == "Á-109" || word == "A-109")
        {
            inGameName = "GameObject - A109";
        }

        //A111
        if (word == "Á.111" || word == "A.111" || word == "Á111" || word == "A111" || word == "Á-111" || word == "A-111")
        {
            inGameName = "GameObject - A111";
        }

        //A113
        if (word == "Á.113" || word == "A.113" || word == "Á113" || word == "A113" || word == "Á-113" || word == "A-113")
        {
            inGameName = "GameObject - A113";
        }

        //A115
        if (word == "Á.115" || word == "A.115" || word == "Á115" || word == "A115" || word == "Á-115" || word == "A-115")
        {
            inGameName = "GameObject - A115";
        }

        //A117
        if (word == "Á.117" || word == "A.117" || word == "Á117" || word == "A117" || word == "Á-117" || word == "A-117")
        {
            inGameName = "GameObject - A117";
        }

        //A119
        if (word == "Á.119" || word == "A.119" || word == "Á119" || word == "A119" || word == "Á-119" || word == "A-119")
        {
            inGameName = "GameObject - A119";
        }

        //A121
        if (word == "Á.121" || word == "A.121" || word == "Á121" || word == "A121" || word == "Á-121" || word == "A-121")
        {
            inGameName = "GameObject - A121";
        }

        //A123
        if (word == "Á.123" || word == "A.123" || word == "Á123" || word == "A123" || word == "Á-123" || word == "A-123")
        {
            inGameName = "GameObject - A123";
        }

        //A125
        if (word == "Á.125" || word == "A.125" || word == "Á125" || word == "A125" || word == "Á-125" || word == "A-125")
        {
            inGameName = "GameObject - A125";
        }



        //B101
        if (word == "Â.101" || word == "B.101" || word == "Â101" || word == "B101" || word == "Â-101" || word == "B-101")
        {
            inGameName = "GameObject - B101";
        }

        //B102
        if (word == "Â.102" || word == "B.102" || word == "Â102" || word == "B102" || word == "Â-102" || word == "B-102")
        {
            inGameName = "GameObject - B102";
        }

        //B103
        if (word == "Â.103" || word == "B.103" || word == "Â103" || word == "B103" || word == "Â-103" || word == "B-103")
        {
            inGameName = "GameObject - B103";
        }

        //B104
        if (word == "Â.104" || word == "B.104" || word == "Â104" || word == "B104" || word == "Â-104" || word == "B-104")
        {
            inGameName = "GameObject - B104";
        }

        //B105
        if (word == "Â.105" || word == "B.105" || word == "Â105" || word == "B105" || word == "Â-105" || word == "B-105")
        {
            inGameName = "GameObject - B105";
        }

        //B106
        if (word == "Â.106" || word == "B.106" || word == "Â106" || word == "B106" || word == "Â-106" || word == "B-106")
        {
            inGameName = "GameObject - B106";
        }

        //B107
        if (word == "Â.107" || word == "B.107" || word == "Â107" || word == "B107" || word == "Â-107" || word == "B-107")
        {
            inGameName = "GameObject - B107";
        }

        //B108
        if (word == "Â.108" || word == "B.108" || word == "Â108" || word == "B108" || word == "Â-108" || word == "B-108")
        {
            inGameName = "GameObject - B108";
        }

        //B109
        if (word == "Â.109" || word == "B.109" || word == "Â109" || word == "B109" || word == "Â-109" || word == "B-109")
        {
            inGameName = "GameObject - B109";
        }

        //B110
        if (word == "Â.110" || word == "B.110" || word == "Â110" || word == "B110" || word == "Â-110" || word == "B-110")
        {
            inGameName = "GameObject - B110";
        }

        //B111
        if (word == "Â.111" || word == "B.111" || word == "Â111" || word == "B111" || word == "Â-111" || word == "B-111")
        {
            inGameName = "GameObject - B111";
        }

        //B112
        if (word == "Â.112" || word == "B.112" || word == "Â112" || word == "B112" || word == "Â-112" || word == "B-112")
        {
            inGameName = "GameObject - B112";
        }

        //B113
        if (word == "Â.113" || word == "B.113" || word == "Â113" || word == "B113" || word == "Â-113" || word == "B-113")
        {
            inGameName = "GameObject - B113";
        }

        //B115
        if (word == "Â.115" || word == "B.115" || word == "Â115" || word == "B115" || word == "Â-115" || word == "B-115")
        {
            inGameName = "GameObject - B115";
        }



        //E101
        if (word == "Å.101" || word == "E.101" || word == "Å101" || word == "E101" || word == "Å-101" || word == "E-101")
        {
            inGameName = "GameObject - E101";
        }

        //E102
        if (word == "Å.102" || word == "E.102" || word == "Å102" || word == "E102" || word == "Å-102" || word == "E-102")
        {
            inGameName = "GameObject - E102";
        }

        //E103
        if (word == "Å.103" || word == "E.103" || word == "Å103" || word == "E103" || word == "Å-103" || word == "E-103")
        {
            inGameName = "GameObject - E103";
        }

        //E104
        if (word == "Å.104" || word == "E.104" || word == "Å104" || word == "E104" || word == "Å-104" || word == "E-104")
        {
            inGameName = "GameObject - E104";
        }

        //E105
        if (word == "Å.105" || word == "E.105" || word == "Å105" || word == "E105" || word == "Å-105" || word == "E-105")
        {
            inGameName = "GameObject - E105";
        }

        //E106
        if (word == "Å.106" || word == "E.106" || word == "Å106" || word == "E106" || word == "Å-106" || word == "E-106")
        {
            inGameName = "GameObject - E106";
        }

        //E108
        if (word == "Å.108" || word == "E.108" || word == "Å108" || word == "E108" || word == "Å-108" || word == "E-108")
        {
            inGameName = "GameObject - E108";
        }

        //E110
        if (word == "Å.110" || word == "E.110" || word == "Å110" || word == "E110" || word == "Å-110" || word == "E-110")
        {
            inGameName = "GameObject - E110";
        }

        //E112
        if (word == "Å.112" || word == "E.112" || word == "Å112" || word == "E112" || word == "Å-112" || word == "E-112")
        {
            inGameName = "GameObject - E112";
        }


        //H101
        if (word == "Ç.101" || word == "H.101" || word == "Ç101" || word == "H101" || word == "Ç-101" || word == "H-101")
        {
            inGameName = "GameObject - H101";
        }

        //H102
        if (word == "Ç.102" || word == "H.102" || word == "Ç102" || word == "H102" || word == "Ç-102" || word == "H-102")
        {
            inGameName = "GameObject - H102";
        }

        //H103
        if (word == "Ç.103" || word == "H.103" || word == "Ç103" || word == "H103" || word == "Ç-103" || word == "H-103")
        {
            inGameName = "GameObject - H103";
        }

        //H104
        if (word == "Ç.104" || word == "H.104" || word == "Ç104" || word == "H104" || word == "Ç-104" || word == "H-104")
        {
            inGameName = "GameObject - H104";
        }

        //H105
        if (word == "Ç.105" || word == "H.105" || word == "Ç105" || word == "H105" || word == "Ç-105" || word == "H-105")
        {
            inGameName = "GameObject - H105";
        }

        //H106
        if (word == "Ç.106" || word == "H.106" || word == "Ç106" || word == "H106" || word == "Ç-106" || word == "H-106")
        {
            inGameName = "GameObject - H106";
        }

        //H107
        if (word == "Ç.107" || word == "H.107" || word == "Ç107" || word == "H107" || word == "Ç-107" || word == "H-107")
        {
            inGameName = "GameObject - H107";
        }

        //H108
        if (word == "Ç.108" || word == "H.108" || word == "Ç108" || word == "H108" || word == "Ç-108" || word == "H-108")
        {
            inGameName = "GameObject - H108";
        }

        //H110
        if (word == "Ç.110" || word == "H.110" || word == "Ç110" || word == "H110" || word == "Ç-110" || word == "H-110")
        {
            inGameName = "GameObject - H110";
        }

        //H112
        if (word == "Ç.112" || word == "H.112" || word == "Ç112" || word == "H112" || word == "Ç-112" || word == "H-112")
        {
            inGameName = "GameObject - H112";
        }

        //H114
        if (word == "Ç.114" || word == "H.114" || word == "Ç114" || word == "H114" || word == "Ç-114" || word == "H-114")
        {
            inGameName = "GameObject - H114";
        }




        //E201
        if (word == "Å.201" || word == "E.201" || word == "Å201" || word == "E201" || word == "Å-201" || word == "E-201")
        {
            inGameName = "GameObject - E201";
        }

        //E202
        if (word == "Å.202" || word == "E.202" || word == "Å202" || word == "E202" || word == "Å-202" || word == "E-202")
        {
            inGameName = "GameObject - E202";
        }

        //E203
        if (word == "Å.203" || word == "E.203" || word == "Å203" || word == "E203" || word == "Å-203" || word == "E-203")
        {
            inGameName = "GameObject - E203";
        }

        //E204
        if (word == "Å.204" || word == "E.204" || word == "Å204" || word == "E204" || word == "Å-204" || word == "E-204")
        {
            inGameName = "GameObject - E204";
        }

        //E205
        if (word == "Å.205" || word == "E.205" || word == "Å205" || word == "E205" || word == "Å-205" || word == "E-205")
        {
            inGameName = "GameObject - E205";
        }

        //E206
        if (word == "Å.206" || word == "E.206" || word == "Å206" || word == "E206" || word == "Å-206" || word == "E-206")
        {
            inGameName = "GameObject - E206";
        }

        //E207
        if (word == "Å.207" || word == "E.207" || word == "Å207" || word == "E207" || word == "Å-207" || word == "E-207")
        {
            inGameName = "GameObject - E207";
        }

        //E208
        if (word == "Å.208" || word == "E.208" || word == "Å208" || word == "E208" || word == "Å-208" || word == "E-208")
        {
            inGameName = "GameObject - E208";
        }

        //E210
        if (word == "Å.210" || word == "E.210" || word == "Å210" || word == "E210" || word == "Å-210" || word == "E-210")
        {
            inGameName = "GameObject - E210";
        }


        //H201
        if (word == "Ç.201" || word == "H.201" || word == "Ç201" || word == "H201" || word == "Ç-201" || word == "H-201")
        {
            inGameName = "GameObject - H201";
        }

        //H202
        if (word == "Ç.202" || word == "H.202" || word == "Ç202" || word == "H202" || word == "Ç-202" || word == "H-202")
        {
            inGameName = "GameObject - H202";
        }

        //H203
        if (word == "Ç.203" || word == "H.203" || word == "Ç203" || word == "H203" || word == "Ç-203" || word == "H-203")
        {
            inGameName = "GameObject - H203";
        }

        //H204
        if (word == "Ç.204" || word == "H.204" || word == "Ç204" || word == "H204" || word == "Ç-204" || word == "H-204")
        {
            inGameName = "GameObject - H204";
        }

        //H205
        if (word == "Ç.205" || word == "H.205" || word == "Ç205" || word == "H205" || word == "Ç-205" || word == "H-205")
        {
            inGameName = "GameObject - H205";
        }

        //H206
        if (word == "Ç.206" || word == "H.206" || word == "Ç206" || word == "H206" || word == "Ç-206" || word == "H-206")
        {
            inGameName = "GameObject - H206";
        }

        //H208
        if (word == "Ç.208" || word == "H.208" || word == "Ç208" || word == "H208" || word == "Ç-208" || word == "H-208")
        {
            inGameName = "GameObject - H208";
        }

        //H210
        if (word == "Ç.210" || word == "H.210" || word == "Ç210" || word == "H210" || word == "Ç-210" || word == "H-210")
        {
            inGameName = "GameObject - H210a";
        }

        //H212
        if (word == "Ç.212" || word == "H.212" || word == "Ç212" || word == "H212" || word == "Ç-212" || word == "H-212")
        {
            inGameName = "GameObject - H212";
        }

        //H214
        if (word == "Ç.214" || word == "H.214" || word == "Ç214" || word == "H214" || word == "Ç-214" || word == "H-214")
        {
            inGameName = "GameObject - H214";
        }

        //H216
        if (word == "Ç.216" || word == "H.216" || word == "Ç216" || word == "H216" || word == "Ç-216" || word == "H-216")
        {
            inGameName = "GameObject - H216";
        }


        //B201
        if (word == "Â.201" || word == "B.201" || word == "Â201" || word == "B201" || word == "Â-201" || word == "B-201")
        {
            inGameName = "GameObject - B201";
        }

        //B202
        if (word == "Â.202" || word == "B.202" || word == "Â202" || word == "B202" || word == "Â-202" || word == "B-202")
        {
            inGameName = "GameObject - B202";
        }

        //B203
        if (word == "Â.203" || word == "B.203" || word == "Â203" || word == "B203" || word == "Â-203" || word == "B-203")
        {
            inGameName = "GameObject - B203";
        }

        //B204
        if (word == "Â.204" || word == "B.204" || word == "Â204" || word == "B204" || word == "Â-204" || word == "B-204")
        {
            inGameName = "GameObject - B204";
        }

        //B205
        if (word == "Â.205" || word == "B.205" || word == "Â205" || word == "B205" || word == "Â-205" || word == "B-205")
        {
            inGameName = "GameObject - B205";
        }

        //B206
        if (word == "Â.206" || word == "B.206" || word == "Â206" || word == "B206" || word == "Â-206" || word == "B-206")
        {
            inGameName = "GameObject - B206";
        }

        //B207
        if (word == "Â.207" || word == "B.207" || word == "Â207" || word == "B207" || word == "Â-207" || word == "B-207")
        {
            inGameName = "GameObject - B207";
        }

        //B208
        if (word == "Â.208" || word == "B.208" || word == "Â208" || word == "B208" || word == "Â-208" || word == "B-208")
        {
            inGameName = "GameObject - B208";
        }

        //B209
        if (word == "Â.209" || word == "B.209" || word == "Â209" || word == "B209" || word == "Â-209" || word == "B-209")
        {
            inGameName = "GameObject - B209";
        }

        //B210
        if (word == "Â.210" || word == "B.210" || word == "Â210" || word == "B210" || word == "Â-210" || word == "B-210")
        {
            inGameName = "GameObject - B210";
        }

        //B211
        if (word == "Â.211" || word == "B.211" || word == "Â211" || word == "B211" || word == "Â-211" || word == "B-211")
        {
            inGameName = "GameObject - B211";
        }

        //B212
        if (word == "Â.212" || word == "B.212" || word == "Â212" || word == "B212" || word == "Â-212" || word == "B-212")
        {
            inGameName = "GameObject - B212";
        }

        //B213
        if (word == "Â.213" || word == "B.213" || word == "Â213" || word == "B213" || word == "Â-213" || word == "B-213")
        {
            inGameName = "GameObject - B213";
        }

        //B214
        if (word == "Â.214" || word == "B.214" || word == "Â214" || word == "B214" || word == "Â-214" || word == "B-214")
        {
            inGameName = "GameObject - B214";
        }

        //B215
        if (word == "Â.215" || word == "B.215" || word == "Â215" || word == "B215" || word == "Â-215" || word == "B-215")
        {
            inGameName = "GameObject - B215";
        }

        //B217
        if (word == "Â.217" || word == "B.217" || word == "Â217" || word == "B217" || word == "Â-217" || word == "B-217")
        {
            inGameName = "GameObject - B217";
        }

        //B219
        if (word == "Â.219" || word == "B.219" || word == "Â219" || word == "B219" || word == "Â-219" || word == "B-219")
        {
            inGameName = "GameObject - B219";
        }

        //B221
        if (word == "Â.221" || word == "B.221" || word == "Â221" || word == "B221" || word == "Â-221" || word == "B-221")
        {
            inGameName = "GameObject - B221";
        }

        //B223
        if (word == "Â.223" || word == "B.223" || word == "Â223" || word == "B223" || word == "Â-223" || word == "B-223")
        {
            inGameName = "GameObject - B223";
        }

        //B225
        if (word == "Â.225" || word == "B.225" || word == "Â225" || word == "B225" || word == "Â-225" || word == "B-225")
        {
            inGameName = "GameObject - B225";
        }

        //B227
        if (word == "Â.227" || word == "B.227" || word == "Â227" || word == "B227" || word == "Â-227" || word == "B-227")
        {
            inGameName = "GameObject - B227";
        }

        //B229
        if (word == "Â.229" || word == "B.229" || word == "Â229" || word == "B229" || word == "Â-229" || word == "B-229")
        {
            inGameName = "GameObject - B229";
        }




        //K201
        if (word == "Ê.201" || word == "K.201" || word == "Ê201" || word == "K201" || word == "Ê-201" || word == "K-201")
        {
            inGameName = "GameObject - K201";
        }

        //K202
        if (word == "Ê.202" || word == "K.202" || word == "Ê202" || word == "K202" || word == "Ê-202" || word == "K-202")
        {
            inGameName = "GameObject - K202";
        }

        //K203
        if (word == "Ê.203" || word == "K.203" || word == "Ê203" || word == "K203" || word == "Ê-203" || word == "K-203")
        {
            inGameName = "GameObject - K203";
        }

        //K204
        if (word == "Ê.204" || word == "K.204" || word == "Ê204" || word == "K204" || word == "Ê-204" || word == "K-204")
        {
            inGameName = "GameObject - K204";
        }

        //K205
        if (word == "Ê.205" || word == "K.205" || word == "Ê205" || word == "K205" || word == "Ê-205" || word == "K-205")
        {
            inGameName = "GameObject - K205";
        }

        //K206
        if (word == "Ê.206" || word == "K.206" || word == "Ê206" || word == "K206" || word == "Ê-206" || word == "K-206")
        {
            inGameName = "GameObject - K206";
        }

        //K207
        if (word == "Ê.207" || word == "K.207" || word == "Ê207" || word == "K207" || word == "Ê-207" || word == "K-207")
        {
            inGameName = "GameObject - K207";
        }

        //K208
        if (word == "Ê.208" || word == "K.208" || word == "Ê208" || word == "K208" || word == "Ê-208" || word == "K-208")
        {
            inGameName = "GameObject - K208";
        }

        //K209
        if (word == "Ê.209" || word == "K.209" || word == "Ê209" || word == "K209" || word == "Ê-209" || word == "K-209")
        {
            inGameName = "GameObject - K209";
        }

        //K210
        if (word == "Ê.210" || word == "K.210" || word == "Ê210" || word == "K210" || word == "Ê-210" || word == "K-210")
        {
            inGameName = "GameObject - K210";
        }

        //K211
        if (word == "Ê.211" || word == "K.211" || word == "Ê211" || word == "K211" || word == "Ê-211" || word == "K-211")
        {
            inGameName = "GameObject - K211";
        }

        //K212
        if (word == "Ê.212" || word == "K.212" || word == "Ê212" || word == "K212" || word == "Ê-212" || word == "K-212")
        {
            inGameName = "GameObject - K212";
        }

        //K213
        if (word == "Ê.213" || word == "K.213" || word == "Ê213" || word == "K213" || word == "Ê-213" || word == "K-213")
        {
            inGameName = "GameObject - K213";
        }

        //K214
        if (word == "Ê.214" || word == "K.214" || word == "Ê214" || word == "K214" || word == "Ê-214" || word == "K-214")
        {
            inGameName = "GameObject - K214";
        }

        //K215
        if (word == "Ê.215" || word == "K.215" || word == "Ê215" || word == "K215" || word == "Ê-215" || word == "K-215")
        {
            inGameName = "GameObject - K215";
        }

        //K216
        if (word == "Ê.216" || word == "K.216" || word == "Ê216" || word == "K216" || word == "Ê-216" || word == "K-216")
        {
            inGameName = "GameObject - K216";
        }

        //K217
        if (word == "Ê.217" || word == "K.217" || word == "Ê217" || word == "K217" || word == "Ê-217" || word == "K-217")
        {
            inGameName = "GameObject - K217";
        }

        //K218
        if (word == "Ê.218" || word == "K.218" || word == "Ê218" || word == "K218" || word == "Ê-218" || word == "K-218")
        {
            inGameName = "GameObject - K218";
        }

        //K219
        if (word == "Ê.219" || word == "K.219" || word == "Ê219" || word == "K219" || word == "Ê-219" || word == "K-219")
        {
            inGameName = "GameObject - K219";
        }

        //K220
        if (word == "Ê.220" || word == "K.220" || word == "Ê220" || word == "K220" || word == "Ê-220" || word == "K-220")
        {
            inGameName = "GameObject - K220";
        }

        //K221
        if (word == "Ê.221" || word == "K.221" || word == "Ê221" || word == "K221" || word == "Ê-221" || word == "K-221")
        {
            inGameName = "GameObject - K221";
        }

        //K223
        if (word == "Ê.223" || word == "K.223" || word == "Ê223" || word == "K223" || word == "Ê-223" || word == "K-223")
        {
            inGameName = "GameObject - K223";
        }

        //K225
        if (word == "Ê.225" || word == "K.225" || word == "Ê225" || word == "K225" || word == "Ê-225" || word == "K-225")
        {
            inGameName = "GameObject - K225";
        }

        //K227
        if (word == "Ê.227" || word == "K.227" || word == "Ê227" || word == "K227" || word == "Ê-227" || word == "K-227")
        {
            inGameName = "GameObject - K227";
        }



        //E301
        if (word == "Å.301" || word == "E.301" || word == "Å301" || word == "E301" || word == "Å-301" || word == "E-301")
        {
            inGameName = "GameObject - E301";
        }

        //E302
        if (word == "Å.302" || word == "E.302" || word == "Å302" || word == "E302" || word == "Å-302" || word == "E-302")
        {
            inGameName = "GameObject - E302";
        }

        //E303
        if (word == "Å.303" || word == "E.303" || word == "Å303" || word == "E303" || word == "Å-303" || word == "E-303")
        {
            inGameName = "GameObject - E303";
        }

        //E304
        if (word == "Å.304" || word == "E.304" || word == "Å304" || word == "E304" || word == "Å-304" || word == "E-304")
        {
            inGameName = "GameObject - E304";
        }

        //E305
        if (word == "Å.305" || word == "E.305" || word == "Å305" || word == "E305" || word == "Å-305" || word == "E-305")
        {
            inGameName = "GameObject - E305";
        }

        //E306
        if (word == "Å.306" || word == "E.306" || word == "Å306" || word == "E306" || word == "Å-306" || word == "E-306")
        {
            inGameName = "GameObject - E306";
        }

        //E307
        if (word == "Å.307" || word == "E.307" || word == "Å307" || word == "E307" || word == "Å-307" || word == "E-307")
        {
            inGameName = "GameObject - E307";
        }

        //E308
        if (word == "Å.308" || word == "E.308" || word == "Å308" || word == "E308" || word == "Å-308" || word == "E-308")
        {
            inGameName = "GameObject - E308";
        }

        //E309
        if (word == "Å.309" || word == "E.309" || word == "Å309" || word == "E309" || word == "Å-309" || word == "E-309")
        {
            inGameName = "GameObject - E309";
        }

        //E310
        if (word == "Å.310" || word == "E.310" || word == "Å310" || word == "E310" || word == "Å-310" || word == "E-310")
        {
            inGameName = "GameObject - E310";
        }

        //E311
        if (word == "Å.311" || word == "E.311" || word == "Å311" || word == "E311" || word == "Å-311" || word == "E-311")
        {
            inGameName = "GameObject - E311";
        }

        //E313
        if (word == "Å.313" || word == "E.313" || word == "Å313" || word == "E313" || word == "Å-313" || word == "E-313")
        {
            inGameName = "GameObject - E313";
        }

        //E315
        if (word == "Å.315" || word == "E.315" || word == "Å315" || word == "E315" || word == "Å-315" || word == "E-315")
        {
            inGameName = "GameObject - E315";
        }

        //E317
        if (word == "Å.317" || word == "E.317" || word == "Å317" || word == "E317" || word == "Å-317" || word == "E-317")
        {
            inGameName = "GameObject - E317";
        }



        //H301
        if (word == "Ç.301" || word == "H.301" || word == "Ç301" || word == "H301" || word == "Ç-301" || word == "H-301")
        {
            inGameName = "GameObject - H301";
        }

        //H302
        if (word == "Ç.302" || word == "H.302" || word == "Ç302" || word == "H302" || word == "Ç-302" || word == "H-302")
        {
            inGameName = "GameObject - H302";
        }

        //H303
        if (word == "Ç.303" || word == "H.303" || word == "Ç303" || word == "H303" || word == "Ç-303" || word == "H-303")
        {
            inGameName = "GameObject - H303";
        }

        //H304
        if (word == "Ç.304" || word == "H.304" || word == "Ç304" || word == "H304" || word == "Ç-304" || word == "H-304")
        {
            inGameName = "GameObject - H304";
        }

        //H305
        if (word == "Ç.305" || word == "H.305" || word == "Ç305" || word == "H305" || word == "Ç-305" || word == "H-305")
        {
            inGameName = "GameObject - H305";
        }

        //H306
        if (word == "Ç.306" || word == "H.306" || word == "Ç306" || word == "H306" || word == "Ç-306" || word == "H-306")
        {
            inGameName = "GameObject - H306";
        }

        //H307
        if (word == "Ç.307" || word == "H.307" || word == "Ç307" || word == "H307" || word == "Ç-307" || word == "H-307")
        {
            inGameName = "GameObject - H307";
        }

        //H308
        if (word == "Ç.308" || word == "H.308" || word == "Ç308" || word == "H308" || word == "Ç-308" || word == "H-308")
        {
            inGameName = "GameObject - H308";
        }

        //H309
        if (word == "Ç.309" || word == "H.309" || word == "Ç309" || word == "H309" || word == "Ç-309" || word == "H-309")
        {
            inGameName = "GameObject - H309";
        }

        //H310
        if (word == "Ç.310" || word == "H.310" || word == "Ç310" || word == "H310" || word == "Ç-310" || word == "H-310")
        {
            inGameName = "GameObject - H310";
        }

        //H311
        if (word == "Ç.311" || word == "H.311" || word == "Ç311" || word == "H311" || word == "Ç-311" || word == "H-311")
        {
            inGameName = "GameObject - H311";
        }

        //H312
        if (word == "Ç.312" || word == "H.312" || word == "Ç312" || word == "H312" || word == "Ç-312" || word == "H-312")
        {
            inGameName = "GameObject - H312";
        }

        //H314
        if (word == "Ç.314" || word == "H.314" || word == "Ç314" || word == "H314" || word == "Ç-314" || word == "H-314")
        {
            inGameName = "GameObject - H314";
        }

        //H316
        if (word == "Ç.316" || word == "H.316" || word == "Ç316" || word == "H316" || word == "Ç-316" || word == "H-316")
        {
            inGameName = "GameObject - H316";
        }


        //B301
        if (word == "Â.301" || word == "B.301" || word == "Â301" || word == "B301" || word == "Â-301" || word == "B-301")
        {
            inGameName = "GameObject - B301";
        }

        //B302
        if (word == "Â.302" || word == "B.302" || word == "Â302" || word == "B302" || word == "Â-302" || word == "B-302")
        {
            inGameName = "GameObject - B302";
        }

        //B303
        if (word == "Â.303" || word == "B.303" || word == "Â303" || word == "B303" || word == "Â-303" || word == "B-303")
        {
            inGameName = "GameObject - B303";
        }

        //B304
        if (word == "Â.304" || word == "B.304" || word == "Â304" || word == "B304" || word == "Â-304" || word == "B-304")
        {
            inGameName = "GameObject - B304";
        }

        //B305
        if (word == "Â.305" || word == "B.305" || word == "Â305" || word == "B305" || word == "Â-305" || word == "B-305")
        {
            inGameName = "GameObject - B305";
        }

        //B306
        if (word == "Â.306" || word == "B.306" || word == "Â306" || word == "B306" || word == "Â-306" || word == "B-306")
        {
            inGameName = "GameObject - B306";
        }

        //B307
        if (word == "Â.307" || word == "B.307" || word == "Â307" || word == "B307" || word == "Â-307" || word == "B-307")
        {
            inGameName = "GameObject - B307";
        }

        //B308
        if (word == "Â.308" || word == "B.308" || word == "Â308" || word == "B308" || word == "Â-308" || word == "B-308")
        {
            inGameName = "GameObject - B308";
        }

        //B309
        if (word == "Â.309" || word == "B.309" || word == "Â309" || word == "B309" || word == "Â-309" || word == "B-309")
        {
            inGameName = "GameObject - B309";
        }

        //B310
        if (word == "Â.310" || word == "B.310" || word == "Â310" || word == "B310" || word == "Â-310" || word == "B-310")
        {
            inGameName = "GameObject - B310";
        }

        //B311
        if (word == "Â.311" || word == "B.311" || word == "Â311" || word == "B311" || word == "Â-311" || word == "B-311")
        {
            inGameName = "GameObject - B311";
        }

        //B312
        if (word == "Â.312" || word == "B.312" || word == "Â312" || word == "B312" || word == "Â-312" || word == "B-312")
        {
            inGameName = "GameObject - B312";
        }

        //B313
        if (word == "Â.313" || word == "B.313" || word == "Â313" || word == "B313" || word == "Â-313" || word == "B-313")
        {
            inGameName = "GameObject - B313";
        }

        //B314
        if (word == "Â.314" || word == "B.314" || word == "Â314" || word == "B314" || word == "Â-314" || word == "B-314")
        {
            inGameName = "GameObject - B314";
        }

        //B315
        if (word == "Â.315" || word == "B.315" || word == "Â315" || word == "B315" || word == "Â-315" || word == "B-315")
        {
            inGameName = "GameObject - B315";
        }

        //B316
        if (word == "Â.316" || word == "B.316" || word == "Â316" || word == "B316" || word == "Â-316" || word == "B-316")
        {
            inGameName = "GameObject - B316";
        }

        //B317
        if (word == "Â.317" || word == "B.317" || word == "Â317" || word == "B317" || word == "Â-317" || word == "B-317")
        {
            inGameName = "GameObject - B317";
        }

        //B318
        if (word == "Â.318" || word == "B.318" || word == "Â318" || word == "B318" || word == "Â-318" || word == "B-318")
        {
            inGameName = "GameObject - B318";
        }

        //B319
        if (word == "Â.319" || word == "B.319" || word == "Â319" || word == "B319" || word == "Â-319" || word == "B-319")
        {
            inGameName = "GameObject - B319";
        }

        //B320
        if (word == "Â.320" || word == "B.320" || word == "Â320" || word == "B320" || word == "Â-320" || word == "B-320")
        {
            inGameName = "GameObject - B320";
        }

        //B321
        if (word == "Â.321" || word == "B.321" || word == "Â321" || word == "B321" || word == "Â-321" || word == "B-321")
        {
            inGameName = "GameObject - B321";
        }

        //B322
        if (word == "Â.322" || word == "B.322" || word == "Â322" || word == "B322" || word == "Â-322" || word == "B-322")
        {
            inGameName = "GameObject - B322";
        }

        //B323
        if (word == "Â.323" || word == "B.323" || word == "Â323" || word == "B323" || word == "Â-323" || word == "B-323")
        {
            inGameName = "GameObject - B323";
        }

        //B325
        if (word == "Â.325" || word == "B.325" || word == "Â325" || word == "B325" || word == "Â-325" || word == "B-325")
        {
            inGameName = "GameObject - B325";
        }

        //B327
        if (word == "Â.327" || word == "B.327" || word == "Â327" || word == "B327" || word == "Â-327" || word == "B-327")
        {
            inGameName = "GameObject - B327";
        }

        //B329
        if (word == "Â.329" || word == "B.329" || word == "Â329" || word == "B329" || word == "Â-329" || word == "B-329")
        {
            inGameName = "GameObject - B329";
        }



        //K301
        if (word == "Ê.301" || word == "K.301" || word == "Ê301" || word == "K301" || word == "Ê-301" || word == "K-301")
        {
            inGameName = "GameObject - K301";
        }

        //K302
        if (word == "Ê.302" || word == "K.302" || word == "Ê302" || word == "K302" || word == "Ê-302" || word == "K-302")
        {
            inGameName = "GameObject - K302";
        }

        //K303
        if (word == "Ê.303" || word == "K.303" || word == "Ê303" || word == "K303" || word == "Ê-303" || word == "K-303")
        {
            inGameName = "GameObject - K303";
        }

        //K304
        if (word == "Ê.304" || word == "K.304" || word == "Ê304" || word == "K304" || word == "Ê-304" || word == "K-304")
        {
            inGameName = "GameObject - K304";
        }

        //K305
        if (word == "Ê.305" || word == "K.305" || word == "Ê305" || word == "K305" || word == "Ê-305" || word == "K-305")
        {
            inGameName = "GameObject - K305";
        }

        //K306
        if (word == "Ê.306" || word == "K.306" || word == "Ê306" || word == "K306" || word == "Ê-306" || word == "K-306")
        {
            inGameName = "GameObject - K306";
        }

        //K307
        if (word == "Ê.307" || word == "K.307" || word == "Ê307" || word == "K307" || word == "Ê-307" || word == "K-307")
        {
            inGameName = "GameObject - K307";
        }

        //K308
        if (word == "Ê.308" || word == "K.308" || word == "Ê308" || word == "K308" || word == "Ê-308" || word == "K-308")
        {
            inGameName = "GameObject - K308";
        }

        //K309
        if (word == "Ê.309" || word == "K.309" || word == "Ê309" || word == "K309" || word == "Ê-309" || word == "K-309")
        {
            inGameName = "GameObject - K309";
        }

        //K310
        if (word == "Ê.310" || word == "K.310" || word == "Ê310" || word == "K310" || word == "Ê-310" || word == "K-310")
        {
            inGameName = "GameObject - K310";
        }

        //K311
        if (word == "Ê.311" || word == "K.311" || word == "Ê311" || word == "K311" || word == "Ê-311" || word == "K-311")
        {
            inGameName = "GameObject - K311";
        }

        //K312
        if (word == "Ê.312" || word == "K.312" || word == "Ê312" || word == "K312" || word == "Ê-312" || word == "K-312")
        {
            inGameName = "GameObject - K312";
        }

        //K313
        if (word == "Ê.313" || word == "K.313" || word == "Ê313" || word == "K313" || word == "Ê-313" || word == "K-313")
        {
            inGameName = "GameObject - K313";
        }

        //K314
        if (word == "Ê.314" || word == "K.314" || word == "Ê314" || word == "K314" || word == "Ê-314" || word == "K-314")
        {
            inGameName = "GameObject - K314";
        }

        //K315
        if (word == "Ê.315" || word == "K.315" || word == "Ê315" || word == "K315" || word == "Ê-315" || word == "K-315")
        {
            inGameName = "GameObject - K315";
        }

        //K316
        if (word == "Ê.316" || word == "B.K316" || word == "Ê316" || word == "K316" || word == "Ê-316" || word == "K-316")
        {
            inGameName = "GameObject - K316";
        }

        //K317
        if (word == "Ê.317" || word == "K.317" || word == "Ê317" || word == "K317" || word == "Ê-317" || word == "K-317")
        {
            inGameName = "GameObject - K317";
        }

        //K319
        if (word == "Ê.319" || word == "K.319" || word == "Ê319" || word == "K319" || word == "Ê-319" || word == "K-319")
        {
            inGameName = "GameObject - K319";
        }

        //K321
        if (word == "Ê.321" || word == "K.321" || word == "Ê321" || word == "K321" || word == "Ê-321" || word == "K-321")
        {
            inGameName = "GameObject - K321";
        }

        //K323
        if (word == "Ê.323" || word == "K.323" || word == "Ê323" || word == "K323" || word == "Ê-323" || word == "K-323")
        {
            inGameName = "GameObject - K323";
        }

        //K325
        if (word == "Ê.325" || word == "K.325" || word == "Ê325" || word == "K325" || word == "Ê-325" || word == "K-325")
        {
            inGameName = "GameObject - K325";
        }

        //K327
        if (word == "Ê.327" || word == "B.K327" || word == "Ê327" || word == "K327" || word == "Ê-327" || word == "K-327")
        {
            inGameName = "GameObject - K327";
        }

        //K329
        if (word == "Ê.329" || word == "K.329" || word == "Ê329" || word == "K329" || word == "Ê-329" || word == "K-329")
        {
            inGameName = "GameObject - K329";
        }

        return inGameName;
    }
}
