using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace GamePlayer//1
{
    public enum E_Player_Type
    {
        Main,
        Other,
    }
    public class PlayerTest : BaseData
    {
        public List<int> list;
        public Dictionary<int, string> dic;
        public int[] arrays;
        public E_Player_Type type;
        public PlayerData player;

        public override int GetBytesNum()
        {
            int num = 0;
            num += 4;//list.Count
            for (int i = 0; i < list.Count; i++)
                num += 4;

            num += 4;//dic.Count
            foreach (int key in dic.Keys)
            {
                num += 4;//key��ռ���ֽ���
                num += 4;//value �ַ������� ռ���ֽ���
                num += Encoding.UTF8.GetByteCount(dic[key]);
            }

            num += 4;//arrays.Length
            for (int i = 0; i < arrays.Length; i++)
                num += 4;

            num += 4;

            num += player.GetBytesNum();

            return num;
        }

        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;

            list = new List<int>();//��ʼ������
            short listCount = ReadShort(bytes, ref index);
            for (int i = 0; i < listCount; i++)
                list.Add(ReadInt(bytes, ref index));

            dic = new Dictionary<int, string>();//��ʼ������
            short dicCount = ReadShort(bytes, ref index);
            for (int i = 0; i < dicCount; i++)
                dic.Add(ReadInt(bytes, ref index), ReadString(bytes, ref index));

            short arraysLength = ReadShort(bytes, ref index);
            arrays = new int[arraysLength];//��ʼ������
            for (int i = 0; i < arraysLength; i++)
                arrays[i] = ReadInt(bytes, ref index);

            type = (E_Player_Type)ReadInt(bytes, ref index);

            player = ReadData<PlayerData>(bytes, ref index);

            return index - beginIndex;
        }

        public override byte[] Writing()
        {
            //�̶�����
            int index = 0;
            byte[] bytes = new byte[GetBytesNum()];

            //�ɱ�� �Ǹ��ݳ�Ա�������������ƴ�ӵ�
            //�洢list�ĳ���
            WriteShort(bytes, (short)list.Count, ref index);
            for (int i = 0; i < list.Count; i++)
                WriteInt(bytes, list[i], ref index);

            //�洢Dictionary������
            //����
            WriteShort(bytes, (short)dic.Count, ref index);
            foreach (int key in dic.Keys)
            {
                WriteInt(bytes, key, ref index);
                WriteString(bytes, dic[key], ref index);
            }

            //�洢����ĳ���
            WriteShort(bytes, (short)arrays.Length, ref index);
            for (int i = 0; i < arrays.Length; i++)
                WriteInt(bytes, arrays[i], ref index);

            //ö��
            WriteInt(bytes, Convert.ToInt32(type), ref index);

            //�Զ������ݽṹ��
            WriteData(bytes, player, ref index);

            //�̶�����
            return bytes;
        }
    }
}

public class GenerateCSharp
{
    //Э�鱣��·��
    private string SAVE_PATH = Application.dataPath + "/Scripts/Protocol/";

    //����ö��
    public void GenerateEnum(XmlNodeList nodes)
    {
        //����ö�ٽű����߼�
        string namespaceStr = "";
        string enumNameStr = "";
        string fieldStr = "";

        foreach (XmlNode enumNode in nodes)
        {
            //��ȡ�����ռ�������Ϣ
            namespaceStr = enumNode.Attributes["namespace"].Value;
            //��ȡö����������Ϣ
            enumNameStr = enumNode.Attributes["name"].Value;
            //��ȡ���е��ֶνڵ� Ȼ������ַ���ƴ��
            XmlNodeList enumFields = enumNode.SelectNodes("field");
            //һ���µ�ö�� ��Ҫ���һ����һ��ƴ�ӵ��ֶ��ַ���
            fieldStr = "";
            foreach (XmlNode enumField in enumFields)
            {
                fieldStr += "\t\t" + enumField.Attributes["name"].Value;
                if (enumField.InnerText != "")
                    fieldStr += " = " + enumField.InnerText;
                fieldStr += ",\r\n";
            }
            //�����пɱ�����ݽ���ƴ��
            string enumStr = $"namespace {namespaceStr}\r\n" +
                             "{\r\n" +
                                $"\tpublic enum {enumNameStr}\r\n" +
                                "\t{\r\n" +
                                    $"{fieldStr}" +
                                "\t}\r\n" +
                             "}";
            //�����ļ���·��
            string path = SAVE_PATH + namespaceStr + "/Enum/";
            //�������������ļ��� �򴴽�
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //�ַ������� �洢Ϊö�ٽű��ļ�
            File.WriteAllText(path + enumNameStr + ".cs", enumStr);
        }

        Debug.Log("ö�����ɽ���");
    }

    //�������ݽṹ��
    public void GenerateData(XmlNodeList nodes)
    {
        string namespaceStr = "";
        string classNameStr = "";
        string fieldStr = "";
        string getBytesNumStr = "";
        string writingStr = "";
        string readingStr = "";

        foreach (XmlNode dataNode in nodes)
        {
            //�����ռ�
            namespaceStr = dataNode.Attributes["namespace"].Value;
            //����
            classNameStr = dataNode.Attributes["name"].Value;
            //��ȡ�����ֶνڵ�
            XmlNodeList fields = dataNode.SelectNodes("field");
            //ͨ������������г�Ա����������ƴ�� ����ƴ�ӽ��
            fieldStr = GetFieldStr(fields);
            //ͨ��ĳ������ ��GetBytesNum�����е��ַ������ݽ���ƴ�� ���ؽ��
            getBytesNumStr = GetGetBytesNumStr(fields);
            //ͨ��ĳ������ ��Writing�����е��ַ������ݽ���ƴ�� ���ؽ��
            writingStr = GetWritingStr(fields);
            //ͨ��ĳ������ ��Reading�����е��ַ������ݽ���ƴ�� ���ؽ��
            readingStr = GetReadingStr(fields);

            string dataStr = "using System;\r\n" +
                             "using System.Collections.Generic;\r\n" +
                             "using System.Text;\r\n" + 
                             $"namespace {namespaceStr}\r\n" +
                              "{\r\n" +
                              $"\tpublic class {classNameStr} : BaseData\r\n" +
                              "\t{\r\n" +
                                    $"{fieldStr}" +
                                    "\t\tpublic override int GetBytesNum()\r\n" +
                                    "\t\t{\r\n" +
                                        "\t\t\tint num = 0;\r\n" +
                                        $"{getBytesNumStr}" +
                                        "\t\t\treturn num;\r\n" +
                                    "\t\t}\r\n" +
                                    "\t\tpublic override byte[] Writing()\r\n" +
                                    "\t\t{\r\n" +
                                        "\t\t\tint index = 0;\r\n"+
                                        "\t\t\tbyte[] bytes = new byte[GetBytesNum()];\r\n" +
                                        $"{writingStr}" +
                                        "\t\t\treturn bytes;\r\n" +
                                    "\t\t}\r\n" +
                                    "\t\tpublic override int Reading(byte[] bytes, int beginIndex = 0)\r\n" +
                                    "\t\t{\r\n" +
                                        "\t\t\tint index = beginIndex;\r\n" +
                                        $"{readingStr}" +
                                        "\t\t\treturn index - beginIndex;\r\n" +
                                    "\t\t}\r\n" +
                              "\t}\r\n" +
                              "}";

            //����Ϊ �ű��ļ�
            //�����ļ���·��
            string path = SAVE_PATH + namespaceStr + "/Data/";
            //�������������ļ��� �򴴽�
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //�ַ������� �洢Ϊö�ٽű��ļ�
            File.WriteAllText(path + classNameStr + ".cs", dataStr);

        }
        Debug.Log("���ݽṹ�����ɽ���");
    }

    //������Ϣ��
    public void GenerateMsg(XmlNodeList nodes)
    {
        string idStr = "";
        string namespaceStr = "";
        string classNameStr = "";
        string fieldStr = "";
        string getBytesNumStr = "";
        string writingStr = "";
        string readingStr = "";

        foreach (XmlNode dataNode in nodes)
        {
            //��ϢID
            idStr = dataNode.Attributes["id"].Value;
            //�����ռ�
            namespaceStr = dataNode.Attributes["namespace"].Value;
            //����
            classNameStr = dataNode.Attributes["name"].Value;
            //��ȡ�����ֶνڵ�
            XmlNodeList fields = dataNode.SelectNodes("field");
            //ͨ������������г�Ա����������ƴ�� ����ƴ�ӽ��
            fieldStr = GetFieldStr(fields);
            //ͨ��ĳ������ ��GetBytesNum�����е��ַ������ݽ���ƴ�� ���ؽ��
            getBytesNumStr = GetGetBytesNumStr(fields);
            //ͨ��ĳ������ ��Writing�����е��ַ������ݽ���ƴ�� ���ؽ��
            writingStr = GetWritingStr(fields);
            //ͨ��ĳ������ ��Reading�����е��ַ������ݽ���ƴ�� ���ؽ��
            readingStr = GetReadingStr(fields);

            string dataStr = "using System;\r\n" +
                             "using System.Collections.Generic;\r\n" +
                             "using System.Text;\r\n" +
                             $"namespace {namespaceStr}\r\n" +
                              "{\r\n" +
                              $"\tpublic class {classNameStr} : BaseMsg\r\n" +
                              "\t{\r\n" +
                                    $"{fieldStr}" +
                                    "\t\tpublic override int GetBytesNum()\r\n" +
                                    "\t\t{\r\n" +
                                        "\t\t\tint num = 8;\r\n" +//���8������� ��ϢID��4���ֽ� + ��Ϣ�峤�ȵ�4���ֽ�
                                        $"{getBytesNumStr}" +
                                        "\t\t\treturn num;\r\n" +
                                    "\t\t}\r\n" +
                                    "\t\tpublic override byte[] Writing()\r\n" +
                                    "\t\t{\r\n" +
                                        "\t\t\tint index = 0;\r\n" +
                                        "\t\t\tbyte[] bytes = new byte[GetBytesNum()];\r\n" +
                                        "\t\t\tWriteInt(bytes, GetID(), ref index);\r\n" +
                                        "\t\t\tWriteInt(bytes, bytes.Length - 8, ref index);\r\n" +
                                        $"{writingStr}" +
                                        "\t\t\treturn bytes;\r\n" +
                                    "\t\t}\r\n" +
                                    "\t\tpublic override int Reading(byte[] bytes, int beginIndex = 0)\r\n" +
                                    "\t\t{\r\n" +
                                        "\t\t\tint index = beginIndex;\r\n" +
                                        $"{readingStr}" +
                                        "\t\t\treturn index - beginIndex;\r\n" +
                                    "\t\t}\r\n" +
                                    "\t\tpublic override int GetID()\r\n" +
                                    "\t\t{\r\n" +
                                        "\t\t\treturn " + idStr + ";\r\n" +
                                    "\t\t}\r\n" +
                              "\t}\r\n" +
                              "}";

            //����Ϊ �ű��ļ�
            //�����ļ���·��
            string path = SAVE_PATH + namespaceStr + "/Msg/";
            //�������������ļ��� �򴴽�
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //�ַ������� �洢Ϊö�ٽű��ļ�
            File.WriteAllText(path + classNameStr + ".cs", dataStr);

        }
        Debug.Log("��Ϣ�����ɽ���");
    }

    /// <summary>
    /// ��ȡ��Ա������������
    /// </summary>
    /// <param name="fields"></param>
    /// <returns></returns>
    private string GetFieldStr(XmlNodeList fields)
    {
        string fieldStr = "";
        foreach (XmlNode field in fields)
        {
            //��������
            string type = field.Attributes["type"].Value;
            //������
            string fieldName = field.Attributes["name"].Value;
            if(type == "list")
            {
                string T = field.Attributes["T"].Value;
                fieldStr += "\t\tpublic List<" + T + "> ";
            }
            else if(type == "array")
            {
                string data = field.Attributes["data"].Value;
                fieldStr += "\t\tpublic " + data + "[] ";
            }
            else if(type == "dic")
            {
                string Tkey = field.Attributes["Tkey"].Value;
                string Tvalue = field.Attributes["Tvalue"].Value;
                fieldStr += "\t\tpublic Dictionary<" + Tkey +  ", " + Tvalue + "> ";
            }
            else if(type == "enum")
            {
                string data = field.Attributes["data"].Value;
                fieldStr += "\t\tpublic " + data + " ";
            }
            else
            {
                fieldStr += "\t\tpublic " + type + " ";
            }

            fieldStr += fieldName + ";\r\n";
        }
        return fieldStr;
    }

    //ƴ�� GetBytesNum�����ķ���
    private string GetGetBytesNumStr(XmlNodeList fields)
    {
        string bytesNumStr = "";

        string type = "";
        string name = "";
        foreach (XmlNode field in fields)
        {
            type = field.Attributes["type"].Value;
            name = field.Attributes["name"].Value;
            if (type == "list")
            {
                string T = field.Attributes["T"].Value;
                bytesNumStr += "\t\t\tnum += 2;\r\n";//+2 ��Ϊ�˽�Լ�ֽ��� ��һ��shortȥ�洢��Ϣ
                bytesNumStr += "\t\t\tfor (int i = 0; i < " + name + ".Count; ++i)\r\n";
                //����ʹ�õ��� name + [i] Ŀ���ǻ�ȡ list���е�Ԫ�ش������ʹ��
                bytesNumStr += "\t\t\t\tnum += " + GetValueBytesNum(T, name + "[i]") + ";\r\n";
            }
            else if (type == "array")
            {
                string data = field.Attributes["data"].Value;
                bytesNumStr += "\t\t\tnum += 2;\r\n";//+2 ��Ϊ�˽�Լ�ֽ��� ��һ��shortȥ�洢��Ϣ
                bytesNumStr += "\t\t\tfor (int i = 0; i < " + name + ".Length; ++i)\r\n";
                //����ʹ�õ��� name + [i] Ŀ���ǻ�ȡ list���е�Ԫ�ش������ʹ��
                bytesNumStr += "\t\t\t\tnum += " + GetValueBytesNum(data, name + "[i]") + ";\r\n";
            }
            else if (type == "dic")
            {
                string Tkey = field.Attributes["Tkey"].Value;
                string Tvalue = field.Attributes["Tvalue"].Value;
                bytesNumStr += "\t\t\tnum += 2;\r\n";//+2 ��Ϊ�˽�Լ�ֽ��� ��һ��shortȥ�洢��Ϣ
                bytesNumStr += "\t\t\tforeach (" + Tkey + " key in " + name + ".Keys)\r\n";
                bytesNumStr += "\t\t\t{\r\n";
                bytesNumStr += "\t\t\t\tnum += " + GetValueBytesNum(Tkey, "key") + ";\r\n";
                bytesNumStr += "\t\t\t\tnum += " + GetValueBytesNum(Tvalue, name + "[key]") + ";\r\n";
                bytesNumStr += "\t\t\t}\r\n";
            }
            else
                bytesNumStr += "\t\t\tnum += " + GetValueBytesNum(type, name) + ";\r\n";
        }

        return bytesNumStr;
    }
    //��ȡ ָ�����͵��ֽ���
    private string GetValueBytesNum(string type, string name)
    {
        //������û��дȫ ���еĳ��ñ������� ����Ը�������ȥ���
        switch (type)
        {
            case "int":
            case "float":
            case "enum":
                return "4";
            case "long":
                return "8";
            case "byte":
            case "bool":
                return "1";
            case "short":
                return "2";
            case "string":
                return "4 + Encoding.UTF8.GetByteCount(" + name + ")";
            default:
                return name + ".GetBytesNum()";
        }
    }

    //ƴ�� Writing�����ķ���
    private string GetWritingStr(XmlNodeList fields)
    {
        string writingStr = "";

        string type = "";
        string name = "";
        foreach (XmlNode field in fields)
        {
            type = field.Attributes["type"].Value;
            name = field.Attributes["name"].Value;
            if(type == "list")
            {
                string T = field.Attributes["T"].Value;
                writingStr += "\t\t\tWriteShort(bytes, (short)" + name + ".Count, ref index);\r\n";
                writingStr += "\t\t\tfor (int i = 0; i < " + name + ".Count; ++i)\r\n";
                writingStr += "\t\t\t\t" + GetFieldWritingStr(T, name + "[i]") + "\r\n";
            }
            else if (type == "array")
            {
                string data = field.Attributes["data"].Value;
                writingStr += "\t\t\tWriteShort(bytes, (short)" + name + ".Length, ref index);\r\n";
                writingStr += "\t\t\tfor (int i = 0; i < " + name + ".Length; ++i)\r\n";
                writingStr += "\t\t\t\t" + GetFieldWritingStr(data, name + "[i]") + "\r\n";
            }
            else if (type == "dic")
            {
                string Tkey = field.Attributes["Tkey"].Value;
                string Tvalue = field.Attributes["Tvalue"].Value;
                writingStr += "\t\t\tWriteShort(bytes, (short)" + name + ".Count, ref index);\r\n";
                writingStr += "\t\t\tforeach (" + Tkey + " key in " + name + ".Keys)\r\n";
                writingStr += "\t\t\t{\r\n";
                writingStr += "\t\t\t\t" + GetFieldWritingStr(Tkey, "key") + "\r\n";
                writingStr += "\t\t\t\t" + GetFieldWritingStr(Tvalue, name + "[key]") + "\r\n";
                writingStr += "\t\t\t}\r\n";
            }
            else
            {
                writingStr += "\t\t\t" + GetFieldWritingStr(type, name) + "\r\n";
            }
        }
        return writingStr;
    }

    private string GetFieldWritingStr(string type, string name)
    {
        switch (type)
        {
            case "byte":
                return "WriteByte(bytes, " + name + ", ref index);";
            case "int":
                return "WriteInt(bytes, " + name + ", ref index);";
            case "short":
                return "WriteShort(bytes, " + name + ", ref index);";
            case "long":
                return "WriteLong(bytes, " + name + ", ref index);";
            case "float":
                return "WriteFloat(bytes, " + name + ", ref index);";
            case "bool":
                return "WriteBool(bytes, " + name + ", ref index);";
            case "string":
                return "WriteString(bytes, " + name + ", ref index);";
            case "enum":
                return "WriteInt(bytes, Convert.ToInt32(" + name + "), ref index);";
            default:
                return "WriteData(bytes, " + name + ", ref index);";
        }
    }

    private string GetReadingStr(XmlNodeList fields)
    {
        string readingStr = "";

        string type = "";
        string name = "";
        foreach (XmlNode field in fields)
        {
            type = field.Attributes["type"].Value;
            name = field.Attributes["name"].Value;
            if (type == "list")
            {
                string T = field.Attributes["T"].Value;
                readingStr += "\t\t\t" + name + " = new List<" + T + ">();\r\n";
                readingStr += "\t\t\tshort " + name + "Count = ReadShort(bytes, ref index);\r\n";
                readingStr += "\t\t\tfor (int i = 0; i < " + name + "Count; ++i)\r\n";
                readingStr += "\t\t\t\t" + name + ".Add(" + GetFieldReadingStr(T) + ");\r\n";
            }
            else if (type == "array")
            {
                string data = field.Attributes["data"].Value;
                readingStr += "\t\t\tshort " + name + "Length = ReadShort(bytes, ref index);\r\n";
                readingStr += "\t\t\t" + name + " = new " + data + "["+ name + "Length];\r\n";
                readingStr += "\t\t\tfor (int i = 0; i < " + name + "Length; ++i)\r\n";
                readingStr += "\t\t\t\t" + name + "[i] = " + GetFieldReadingStr(data) + ";\r\n";
            }
            else if (type == "dic")
            {
                string Tkey = field.Attributes["Tkey"].Value;
                string Tvalue = field.Attributes["Tvalue"].Value;
                readingStr += "\t\t\t" + name + " = new Dictionary<" + Tkey + ", " + Tvalue + ">();\r\n";
                readingStr += "\t\t\tshort " + name + "Count = ReadShort(bytes, ref index);\r\n";
                readingStr += "\t\t\tfor (int i = 0; i < " + name + "Count; ++i)\r\n";
                readingStr += "\t\t\t\t" + name + ".Add(" + GetFieldReadingStr(Tkey) + ", " +
                                                            GetFieldReadingStr(Tvalue) + ");\r\n";
            }
            else if (type == "enum")
            {
                string data = field.Attributes["data"].Value;
                readingStr += "\t\t\t" + name + " = (" + data + ")ReadInt(bytes, ref index);\r\n";
            }
            else
                readingStr += "\t\t\t" + name + " = " + GetFieldReadingStr(type) + ";\r\n";
        }

        return readingStr;
    }

    private string GetFieldReadingStr(string type)
    {
        switch (type)
        {
            case "byte":
                return "ReadByte(bytes, ref index)";
            case "int":
                return "ReadInt(bytes, ref index)";
            case "short":
                return "ReadShort(bytes, ref index)";
            case "long":
                return "ReadLong(bytes, ref index)";
            case "float":
                return "ReadFloat(bytes, ref index)";
            case "bool":
                return "ReadBool(bytes, ref index)";
            case "string":
                return "ReadString(bytes, ref index)";
            default:
                return "ReadData<" + type + ">(bytes, ref index)";
        }
    }
}
