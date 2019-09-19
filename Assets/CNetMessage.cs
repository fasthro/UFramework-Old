using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;


public class CNetMessage
{
    //协议ID
    int m_nMessageID;
    //SessionID
    int m_nSessionID;
    //当前操作的Byte数组的位置
    int m_nCurrentPos;
    //缓冲区大小
    public const int BUFFERSIZE = 8192;
    //消息内容字节数组
    byte[] m_btSendBuffer = new byte[BUFFERSIZE];
    //协议长度字节数组
    byte[] m_btMsgLength = new byte[4];
    //协议头字节数组
    byte[] m_btMsgHeader = new byte[4];
    //协议ID字节数组
    byte[] m_btMsgID = new byte[4];
    //Session字节数组
    byte[] m_btSessionID = new byte[4];
    //消息体的字节数字
    byte[] m_btMsgBody = new byte[BUFFERSIZE];
    //是否使用大字节序
    private bool m_bIsBigEndian;
    //从Socket接受到的字节数组
    byte[] m_btReceivBuffer = new byte[65535];
    //协议的状态
    byte btStatus;

    byte btIsResend;

    public CNetMessage()
    {
        //目前默认使用大字节序
        m_bIsBigEndian = true;

        m_nMessageID = 0;
        m_nSessionID = 0;
        m_nCurrentPos = 0;

        //协议头 4个字节 虽然是char，可是目前按照byte来算
        int nIndex = 0;
        Array.Copy(BitConverter.GetBytes('R'), 0, m_btMsgHeader, nIndex, sizeof(byte));
        nIndex += sizeof(byte);
        Array.Copy(BitConverter.GetBytes('H'), 0, m_btMsgHeader, nIndex, sizeof(byte));
        nIndex += sizeof(byte);
        Array.Copy(BitConverter.GetBytes('E'), 0, m_btMsgHeader, nIndex, sizeof(byte));
        nIndex += sizeof(byte);
        Array.Copy(BitConverter.GetBytes('A'), 0, m_btMsgHeader, nIndex, sizeof(byte));

        btIsResend = 0;
    }

    public int ActualLenght
    {
        get
        {
            return m_nCurrentPos;
        }
    }

    public byte IsResend
    {
        get { return btIsResend; }
        set { btIsResend = value; }
    }

    public int MessageID
    {
        set
        {
            m_nMessageID = value;
            //消息ID 4个字节
            Array.Copy(BitConverter.GetBytes(m_nMessageID), 0, m_btMsgID, 0, sizeof(int));
            //如果是当前系统是小字节序，并且我们需要使用大字节序，那么就转一下
            if (BitConverter.IsLittleEndian && m_bIsBigEndian)
            {
                Array.Reverse(m_btMsgID);
            }
        }
        get
        {
            return m_nMessageID;
        }
    }

    public int SessionID
    {
        set
        {
            m_nSessionID = value;
            //消息ID 4个字节
            Array.Copy(BitConverter.GetBytes(m_nSessionID), 0, m_btSessionID, 0, sizeof(int));
            //如果是当前系统是小字节序，并且我们需要使用大字节序，那么就转一下
            if (BitConverter.IsLittleEndian && m_bIsBigEndian)
            {
                Array.Reverse(m_btSessionID);
            }
        }
        get
        {
            return m_nSessionID;
        }
    }

    public byte MessageStatus
    {
        get { return btStatus; }
        set { btStatus = value; }
    }

    private string errorStr;
    public string ErrorStr
    {
        get { return errorStr; }
        set { errorStr = value; }
    }


    public void AddByte(byte btNumber)
    {
        Array.Copy(BitConverter.GetBytes(btNumber), 0, m_btMsgBody, m_nCurrentPos, sizeof(byte));
        m_nCurrentPos += sizeof(byte);
    }

    public void AddShort(short sNumber)
    {
        byte[] temp = new byte[sizeof(short)];
        Array.Copy(BitConverter.GetBytes(sNumber), 0, temp, 0, sizeof(short));
        //如果是当前系统是小字节序，并且我们需要使用大字节序，那么就转一下
        if (BitConverter.IsLittleEndian && m_bIsBigEndian)
        {
            Array.Reverse(temp);
        }
        //拷贝到消息体数组
        Array.Copy(temp, 0, m_btMsgBody, m_nCurrentPos, temp.Length);
        //增加索引计数
        m_nCurrentPos += sizeof(short);
    }

    public void AddInt(int nNumber)
    {
        byte[] temp = new byte[sizeof(int)];
        Array.Copy(BitConverter.GetBytes(nNumber), 0, temp, 0, sizeof(int));
        //如果是当前系统是小字节序，并且我们需要使用大字节序，那么就转一下
        if (BitConverter.IsLittleEndian && m_bIsBigEndian)
        {
            Array.Reverse(temp);
        }
        //拷贝到消息体数组
        Array.Copy(temp, 0, m_btMsgBody, m_nCurrentPos, temp.Length);
        //增加索引计数
        m_nCurrentPos += sizeof(int);
    }

    public void AddDouble(double dNumber)
    {
        byte[] temp = new byte[sizeof(double)];
        Array.Copy(BitConverter.GetBytes(dNumber), 0, temp, 0, sizeof(double));
        //如果是当前系统是小字节序，并且我们需要使用大字节序，那么就转一下
        if (BitConverter.IsLittleEndian && m_bIsBigEndian)
        {
            Array.Reverse(temp);
        }
        //拷贝到消息体数组
        Array.Copy(temp, 0, m_btMsgBody, m_nCurrentPos, temp.Length);
        //增加索引计数
        m_nCurrentPos += sizeof(double);
    }

    public void AddLong(long lNumber)
    {
        byte[] temp = new byte[sizeof(long)];
        Array.Copy(BitConverter.GetBytes(lNumber), 0, temp, 0, sizeof(long));
        //如果是当前系统是小字节序，并且我们需要使用大字节序，那么就转一下
        if (BitConverter.IsLittleEndian && m_bIsBigEndian)
        {
            Array.Reverse(temp);
        }
        //拷贝到消息体数组
        Array.Copy(temp, 0, m_btMsgBody, m_nCurrentPos, temp.Length);
        //增加索引计数
        m_nCurrentPos += sizeof(long);
    }

    public void AddString(string strString)
    {
        byte[] btString = (Encoding.UTF8.GetBytes(strString));
        byte[] btStringLength = BitConverter.GetBytes(btString.Length);

        //如果是当前系统是小字节序，并且我们需要使用大字节序，那么就转一下，对于字符串，只需要转长度即可，字符串本身不转
        if (BitConverter.IsLittleEndian && m_bIsBigEndian)
        {
            Array.Reverse(btStringLength);
        }

        byte[] btStringAll = new byte[btStringLength.Length + btString.Length];

        int nStrPos = 0;
        Array.Copy(btStringLength, 0, btStringAll, nStrPos, btStringLength.Length);
        nStrPos += btStringLength.Length;

        Array.Copy(btString, 0, btStringAll, nStrPos, btString.Length);
        nStrPos += btString.Length;

        //拷贝到消息体数组
        Array.Copy(btStringAll, 0, m_btMsgBody, m_nCurrentPos, btStringAll.Length);
        //增加索引计数
        m_nCurrentPos += btStringAll.Length;
    }

    public byte[] Bytes
    {
        get
        {
            AssmbleMessage();
            return m_btSendBuffer;
        }
    }

    public void AssmbleMessage()
    {
        int nPos = 0;
        byte[] btMsgAll = new byte[m_btMsgLength.Length + m_btMsgHeader.Length + m_btMsgID.Length + m_btSessionID.Length + m_nCurrentPos];



        //协议长度4个字节(int)不算协议长度本身
        int nLength = btMsgAll.Length - m_btMsgLength.Length;
        Array.Copy(BitConverter.GetBytes(nLength), 0, m_btMsgLength, 0, sizeof(int));
        //如果是当前系统是小字节序，并且我们需要使用大字节序，那么就转一下
        if (BitConverter.IsLittleEndian && m_bIsBigEndian)
        {
            Array.Reverse(m_btMsgLength);
        }
        Array.Copy(m_btMsgLength, 0, btMsgAll, nPos, m_btMsgLength.Length);
        nPos += m_btMsgLength.Length;

        Array.Copy(m_btMsgHeader, 0, btMsgAll, nPos, m_btMsgHeader.Length);
        nPos += m_btMsgHeader.Length;

        Array.Copy(m_btMsgID, 0, btMsgAll, nPos, m_btMsgID.Length);
        nPos += m_btMsgID.Length;

        Array.Copy(m_btSessionID, 0, btMsgAll, nPos, m_btSessionID.Length);
        nPos += m_btSessionID.Length;
        Array.Copy(m_btMsgBody, 0, btMsgAll, nPos, m_nCurrentPos);
        nPos += btMsgAll.Length;

        m_btSendBuffer = btMsgAll;
    }

    //将收到的字节数组转化并读出相应的协议长度，包头，消息ID和SessionID
    public void ConvertBytes(byte[] btData)
    {
        m_nCurrentPos = 0;
        if (btData == null)
            Debug.LogError("ConvertBytes btData = Null");

        if (btData.Length < 1)
            Debug.LogError("ConvertBytes btData size < 1");

        Array.Copy(btData, 0, m_btReceivBuffer, 0, btData.Length);

        //协议长度
        //int nMessageLegth = ReadInt();
        //包头4个byte;
        byte a = ReadByte();
        byte b = ReadByte();
        byte c = ReadByte();
        byte d = ReadByte();
        //消息ID
        MessageID = ReadInt();
        //SessionID
        SessionID = ReadInt();
    }

    public byte ReadByte()
    {
        byte btBytes = m_btReceivBuffer[m_nCurrentPos];
        m_nCurrentPos++;
        return btBytes;
    }

    public short ReadShort()
    {
        //一个临时缓冲区，如果有大小头序的问题，那么用来转置
        byte[] btTempBuffer = new byte[sizeof(short)];
        Array.Copy(m_btReceivBuffer, m_nCurrentPos, btTempBuffer, 0, sizeof(short));
        //如果是当前系统是小字节序，并且我们需要使用大字节序，那么就转一下，对于字符串，只需要转长度即可，字符串本身不转
        if (BitConverter.IsLittleEndian && m_bIsBigEndian)
        {
            Array.Reverse(btTempBuffer);
        }
        m_nCurrentPos += sizeof(short);
        return BitConverter.ToInt16(btTempBuffer, 0);
    }

    public int ReadInt()
    {
        //一个临时缓冲区，如果有大小头序的问题，那么用来转置
        byte[] btTempBuffer = new byte[sizeof(int)];
        Array.Copy(m_btReceivBuffer, m_nCurrentPos, btTempBuffer, 0, sizeof(int));
        //如果是当前系统是小字节序，并且我们需要使用大字节序，那么就转一下，对于字符串，只需要转长度即可，字符串本身不转
        if (BitConverter.IsLittleEndian && m_bIsBigEndian)
        {
            Array.Reverse(btTempBuffer);
        }
        m_nCurrentPos += sizeof(int);
        return BitConverter.ToInt32(btTempBuffer, 0);
    }

    public long ReadLong()
    {
        //一个临时缓冲区，如果有大小头序的问题，那么用来转置
        byte[] btTempBuffer = new byte[sizeof(long)];
        Array.Copy(m_btReceivBuffer, m_nCurrentPos, btTempBuffer, 0, sizeof(long));
        //如果是当前系统是小字节序，并且我们需要使用大字节序，那么就转一下，对于字符串，只需要转长度即可，字符串本身不转
        if (BitConverter.IsLittleEndian && m_bIsBigEndian)
        {
            Array.Reverse(btTempBuffer);
        }
        m_nCurrentPos += sizeof(long);
        return BitConverter.ToInt64(btTempBuffer, 0);
    }

    public double ReadDouble()
    {
        //一个临时缓冲区，如果有大小头序的问题，那么用来转置
        byte[] btTempBuffer = new byte[sizeof(double)];
        Array.Copy(m_btReceivBuffer, m_nCurrentPos, btTempBuffer, 0, sizeof(double));
        //如果是当前系统是小字节序，并且我们需要使用大字节序，那么就转一下，对于字符串，只需要转长度即可，字符串本身不转
        if (BitConverter.IsLittleEndian && m_bIsBigEndian)
        {
            Array.Reverse(btTempBuffer);
        }
        m_nCurrentPos += sizeof(double);
        return BitConverter.ToDouble(btTempBuffer, 0);
    }

    public string ReadString()
    {
        //先读取字符串的长短
        byte[] btStringLength = new byte[sizeof(int)];
        Array.Copy(m_btReceivBuffer, m_nCurrentPos, btStringLength, 0, sizeof(int));
        if (BitConverter.IsLittleEndian && m_bIsBigEndian)
        {
            Array.Reverse(btStringLength);
        }
        int nStringLength = BitConverter.ToInt32(btStringLength, 0);
        m_nCurrentPos += sizeof(int);
        //读取字符串
        byte[] btString = new byte[nStringLength];
        Array.Copy(m_btReceivBuffer, m_nCurrentPos, btString, 0, nStringLength);
        //Debug.LogError("转换前的字节数:" + btString.Length);
        m_nCurrentPos += nStringLength;

        return Encoding.UTF8.GetString(btString, 0, nStringLength);
    }

    public static void CopyTo(Stream src, Stream dest, int nLength)
    {
        byte[] bytes = new byte[nLength];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        {
            dest.Write(bytes, 0, cnt);
        }
    }

    public override bool Equals(object obj)
    {
        var msg = (CNetMessage)obj;
        if (msg == null)
        {
            return false;
        }
        if (msg.SessionID != SessionID)
        {
            return false;
        }
        if (msg.Bytes.Length != Bytes.Length)
        {
            return false;
        }
        for (int i = 0; i < msg.Bytes.Length; i++)
        {
            if (msg.Bytes[i] != Bytes[i])
            {
                return false;
            }
        }
        return true;
    }

    public void ReWind()
    {
        ConvertBytes(m_btReceivBuffer);
    }
}
