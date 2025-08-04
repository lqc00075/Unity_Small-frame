using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class NetTool 
{
    //���л�Protobuf���ɵĶ���
    public static byte[] GetProtoBytes( IMessage msg )
    {
        //��չ�����������滻���ӿ� ��Щ֪ʶ�� ���� C#��ص����ݵ���

        //����д�� �����Ͻڿ�ѧϰ��֪ʶ��
        //byte[] bytes = null;
        //using (MemoryStream ms = new MemoryStream())
        //{
        //    msg.WriteTo(ms);
        //    bytes = ms.ToArray();
        //}
        //return bytes;

        //ͨ������չ���� �Ϳ���ֱ�ӻ�ȡ��Ӧ����� �ֽ�������
        return msg.ToByteArray();
    }

    /// <summary>
    /// �����л��ֽ�����ΪProtobuf��صĶ���
    /// </summary>
    /// <typeparam name="T">��Ҫ��ȡ����Ϣ����</typeparam>
    /// <param name="bytes">��Ӧ���ֽ����� ���ڷ����л�</param>
    /// <returns></returns>
    public static T GetProtoMsg<T>(byte[] bytes) where T:class, IMessage
    {
        //���� C#����
        //���� C#����
        //�õ���Ӧ��Ϣ������ ͨ������õ��ڲ��ľ�̬��Ա Ȼ��õ����е� ��Ӧ����
        //���з����л�
        Type type = typeof(T);
        //ͨ������ �õ���Ӧ�� ��̬��Ա���Զ���
        PropertyInfo pInfo = type.GetProperty("Parser");
        object parserObj = pInfo.GetValue(null, null);
        //�Ѿ��õ��˶��� ��ô���Եõ��ö����е� ��Ӧ���� 
        Type parserType = parserObj.GetType();
        //����ָ���õ�ĳһ�����غ���
        MethodInfo mInfo = parserType.GetMethod("ParseFrom", new Type[] { typeof(byte[]) });
        //���ö�Ӧ�ķ��� �����л�Ϊָ���Ķ���
        object msg = mInfo.Invoke(parserObj, new object[] { bytes });
        return msg as T;
    }
}
