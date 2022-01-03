// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipOutput
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Ionic.Zip
{
  internal static class ZipOutput
  {
    public static bool WriteCentralDirectoryStructure(
      Stream s,
      ICollection<ZipEntry> entries,
      uint numSegments,
      Zip64Option zip64,
      string comment,
      ZipContainer container)
    {
      if (s is ZipSegmentedStream zipSegmentedStream)
        zipSegmentedStream.ContiguousWrite = true;
      long num1 = 0;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        foreach (ZipEntry entry in (IEnumerable<ZipEntry>) entries)
        {
          if (entry.IncludedInMostRecentSave)
            entry.WriteCentralDirectoryEntry((Stream) memoryStream);
        }
        byte[] array = memoryStream.ToArray();
        s.Write(array, 0, array.Length);
        num1 = (long) array.Length;
      }
      long EndOfCentralDirectory = s is CountingStream countingStream ? countingStream.ComputedPosition : s.Position;
      long StartOfCentralDirectory = EndOfCentralDirectory - num1;
      uint num2 = zipSegmentedStream != null ? zipSegmentedStream.CurrentSegment : 0U;
      long num3 = EndOfCentralDirectory - StartOfCentralDirectory;
      int entryCount = ZipOutput.CountEntries(entries);
      bool flag = zip64 == Zip64Option.Always || entryCount >= (int) ushort.MaxValue || num3 > (long) uint.MaxValue || StartOfCentralDirectory > (long) uint.MaxValue;
      byte[] buffer1;
      if (flag)
      {
        if (zip64 == Zip64Option.Default)
        {
          if (new StackFrame(1).GetMethod().DeclaringType == typeof (ZipFile))
            throw new ZipException("The archive requires a ZIP64 Central Directory. Consider setting the ZipFile.UseZip64WhenSaving property.");
          throw new ZipException("The archive requires a ZIP64 Central Directory. Consider setting the ZipOutputStream.EnableZip64 property.");
        }
        byte[] buffer2 = ZipOutput.GenZip64EndOfCentralDirectory(StartOfCentralDirectory, EndOfCentralDirectory, entryCount, numSegments);
        buffer1 = ZipOutput.GenCentralDirectoryFooter(StartOfCentralDirectory, EndOfCentralDirectory, zip64, entryCount, comment, container);
        if (num2 > 0U)
        {
          uint segment = zipSegmentedStream.ComputeSegment(buffer2.Length + buffer1.Length);
          int destinationIndex1 = 16;
          Array.Copy((Array) BitConverter.GetBytes(segment), 0, (Array) buffer2, destinationIndex1, 4);
          int destinationIndex2 = destinationIndex1 + 4;
          Array.Copy((Array) BitConverter.GetBytes(segment), 0, (Array) buffer2, destinationIndex2, 4);
          int destinationIndex3 = 60;
          Array.Copy((Array) BitConverter.GetBytes(segment), 0, (Array) buffer2, destinationIndex3, 4);
          int destinationIndex4 = destinationIndex3 + 4 + 8;
          Array.Copy((Array) BitConverter.GetBytes(segment), 0, (Array) buffer2, destinationIndex4, 4);
        }
        s.Write(buffer2, 0, buffer2.Length);
      }
      else
        buffer1 = ZipOutput.GenCentralDirectoryFooter(StartOfCentralDirectory, EndOfCentralDirectory, zip64, entryCount, comment, container);
      if (num2 > 0U)
      {
        ushort segment = (ushort) zipSegmentedStream.ComputeSegment(buffer1.Length);
        int destinationIndex5 = 4;
        Array.Copy((Array) BitConverter.GetBytes(segment), 0, (Array) buffer1, destinationIndex5, 2);
        int destinationIndex6 = destinationIndex5 + 2;
        Array.Copy((Array) BitConverter.GetBytes(segment), 0, (Array) buffer1, destinationIndex6, 2);
        int num4 = destinationIndex6 + 2;
      }
      s.Write(buffer1, 0, buffer1.Length);
      if (zipSegmentedStream != null)
        zipSegmentedStream.ContiguousWrite = false;
      return flag;
    }

    private static Encoding GetEncoding(ZipContainer container, string t)
    {
      switch (container.AlternateEncodingUsage)
      {
        case ZipOption.Default:
          return container.DefaultEncoding;
        case ZipOption.Always:
          return container.AlternateEncoding;
        default:
          Encoding defaultEncoding = container.DefaultEncoding;
          if (t == null)
            return defaultEncoding;
          byte[] bytes = defaultEncoding.GetBytes(t);
          return defaultEncoding.GetString(bytes, 0, bytes.Length).Equals(t) ? defaultEncoding : container.AlternateEncoding;
      }
    }

    private static byte[] GenCentralDirectoryFooter(
      long StartOfCentralDirectory,
      long EndOfCentralDirectory,
      Zip64Option zip64,
      int entryCount,
      string comment,
      ZipContainer container)
    {
      Encoding encoding = ZipOutput.GetEncoding(container, comment);
      int num1 = 22;
      byte[] numArray1 = (byte[]) null;
      short num2 = 0;
      if (comment != null && (uint) comment.Length > 0U)
      {
        numArray1 = encoding.GetBytes(comment);
        num2 = (short) numArray1.Length;
      }
      byte[] numArray2 = new byte[num1 + (int) num2];
      int destinationIndex = 0;
      Array.Copy((Array) BitConverter.GetBytes(101010256U), 0, (Array) numArray2, destinationIndex, 4);
      int num3 = destinationIndex + 4;
      byte[] numArray3 = numArray2;
      int index1 = num3;
      int num4 = index1 + 1;
      numArray3[index1] = (byte) 0;
      byte[] numArray4 = numArray2;
      int index2 = num4;
      int num5 = index2 + 1;
      numArray4[index2] = (byte) 0;
      byte[] numArray5 = numArray2;
      int index3 = num5;
      int num6 = index3 + 1;
      numArray5[index3] = (byte) 0;
      byte[] numArray6 = numArray2;
      int index4 = num6;
      int num7 = index4 + 1;
      numArray6[index4] = (byte) 0;
      if (entryCount >= (int) ushort.MaxValue || zip64 == Zip64Option.Always)
      {
        for (int index5 = 0; index5 < 4; ++index5)
          numArray2[num7++] = byte.MaxValue;
      }
      else
      {
        byte[] numArray7 = numArray2;
        int index6 = num7;
        int num8 = index6 + 1;
        int num9 = (int) (byte) (entryCount & (int) byte.MaxValue);
        numArray7[index6] = (byte) num9;
        byte[] numArray8 = numArray2;
        int index7 = num8;
        int num10 = index7 + 1;
        int num11 = (int) (byte) ((entryCount & 65280) >> 8);
        numArray8[index7] = (byte) num11;
        byte[] numArray9 = numArray2;
        int index8 = num10;
        int num12 = index8 + 1;
        int num13 = (int) (byte) (entryCount & (int) byte.MaxValue);
        numArray9[index8] = (byte) num13;
        byte[] numArray10 = numArray2;
        int index9 = num12;
        num7 = index9 + 1;
        int num14 = (int) (byte) ((entryCount & 65280) >> 8);
        numArray10[index9] = (byte) num14;
      }
      long num15 = EndOfCentralDirectory - StartOfCentralDirectory;
      if (num15 >= (long) uint.MaxValue || StartOfCentralDirectory >= (long) uint.MaxValue)
      {
        for (int index10 = 0; index10 < 8; ++index10)
          numArray2[num7++] = byte.MaxValue;
      }
      else
      {
        byte[] numArray11 = numArray2;
        int index11 = num7;
        int num16 = index11 + 1;
        int num17 = (int) (byte) ((ulong) num15 & (ulong) byte.MaxValue);
        numArray11[index11] = (byte) num17;
        byte[] numArray12 = numArray2;
        int index12 = num16;
        int num18 = index12 + 1;
        int num19 = (int) (byte) ((num15 & 65280L) >> 8);
        numArray12[index12] = (byte) num19;
        byte[] numArray13 = numArray2;
        int index13 = num18;
        int num20 = index13 + 1;
        int num21 = (int) (byte) ((num15 & 16711680L) >> 16);
        numArray13[index13] = (byte) num21;
        byte[] numArray14 = numArray2;
        int index14 = num20;
        int num22 = index14 + 1;
        int num23 = (int) (byte) ((num15 & 4278190080L) >> 24);
        numArray14[index14] = (byte) num23;
        byte[] numArray15 = numArray2;
        int index15 = num22;
        int num24 = index15 + 1;
        int num25 = (int) (byte) ((ulong) StartOfCentralDirectory & (ulong) byte.MaxValue);
        numArray15[index15] = (byte) num25;
        byte[] numArray16 = numArray2;
        int index16 = num24;
        int num26 = index16 + 1;
        int num27 = (int) (byte) ((StartOfCentralDirectory & 65280L) >> 8);
        numArray16[index16] = (byte) num27;
        byte[] numArray17 = numArray2;
        int index17 = num26;
        int num28 = index17 + 1;
        int num29 = (int) (byte) ((StartOfCentralDirectory & 16711680L) >> 16);
        numArray17[index17] = (byte) num29;
        byte[] numArray18 = numArray2;
        int index18 = num28;
        num7 = index18 + 1;
        int num30 = (int) (byte) ((StartOfCentralDirectory & 4278190080L) >> 24);
        numArray18[index18] = (byte) num30;
      }
      int num31;
      if (comment == null || comment.Length == 0)
      {
        byte[] numArray19 = numArray2;
        int index19 = num7;
        int num32 = index19 + 1;
        numArray19[index19] = (byte) 0;
        byte[] numArray20 = numArray2;
        int index20 = num32;
        num31 = index20 + 1;
        numArray20[index20] = (byte) 0;
      }
      else
      {
        if ((int) num2 + num7 + 2 > numArray2.Length)
          num2 = (short) (numArray2.Length - num7 - 2);
        byte[] numArray21 = numArray2;
        int index21 = num7;
        int num33 = index21 + 1;
        int num34 = (int) (byte) ((uint) num2 & (uint) byte.MaxValue);
        numArray21[index21] = (byte) num34;
        byte[] numArray22 = numArray2;
        int index22 = num33;
        int num35 = index22 + 1;
        int num36 = (int) (byte) (((int) num2 & 65280) >> 8);
        numArray22[index22] = (byte) num36;
        if ((uint) num2 > 0U)
        {
          int index23;
          for (index23 = 0; index23 < (int) num2 && num35 + index23 < numArray2.Length; ++index23)
            numArray2[num35 + index23] = numArray1[index23];
          num31 = num35 + index23;
        }
      }
      return numArray2;
    }

    private static byte[] GenZip64EndOfCentralDirectory(
      long StartOfCentralDirectory,
      long EndOfCentralDirectory,
      int entryCount,
      uint numSegments)
    {
      byte[] numArray1 = new byte[76];
      int destinationIndex1 = 0;
      Array.Copy((Array) BitConverter.GetBytes(101075792U), 0, (Array) numArray1, destinationIndex1, 4);
      int destinationIndex2 = destinationIndex1 + 4;
      Array.Copy((Array) BitConverter.GetBytes(44L), 0, (Array) numArray1, destinationIndex2, 8);
      int num1 = destinationIndex2 + 8;
      byte[] numArray2 = numArray1;
      int index1 = num1;
      int num2 = index1 + 1;
      numArray2[index1] = (byte) 45;
      byte[] numArray3 = numArray1;
      int index2 = num2;
      int num3 = index2 + 1;
      numArray3[index2] = (byte) 0;
      byte[] numArray4 = numArray1;
      int index3 = num3;
      int num4 = index3 + 1;
      numArray4[index3] = (byte) 45;
      byte[] numArray5 = numArray1;
      int index4 = num4;
      int destinationIndex3 = index4 + 1;
      numArray5[index4] = (byte) 0;
      for (int index5 = 0; index5 < 8; ++index5)
        numArray1[destinationIndex3++] = (byte) 0;
      long num5 = (long) entryCount;
      Array.Copy((Array) BitConverter.GetBytes(num5), 0, (Array) numArray1, destinationIndex3, 8);
      int destinationIndex4 = destinationIndex3 + 8;
      Array.Copy((Array) BitConverter.GetBytes(num5), 0, (Array) numArray1, destinationIndex4, 8);
      int destinationIndex5 = destinationIndex4 + 8;
      Array.Copy((Array) BitConverter.GetBytes(EndOfCentralDirectory - StartOfCentralDirectory), 0, (Array) numArray1, destinationIndex5, 8);
      int destinationIndex6 = destinationIndex5 + 8;
      Array.Copy((Array) BitConverter.GetBytes(StartOfCentralDirectory), 0, (Array) numArray1, destinationIndex6, 8);
      int destinationIndex7 = destinationIndex6 + 8;
      Array.Copy((Array) BitConverter.GetBytes(117853008U), 0, (Array) numArray1, destinationIndex7, 4);
      int destinationIndex8 = destinationIndex7 + 4;
      Array.Copy((Array) BitConverter.GetBytes(numSegments == 0U ? 0U : numSegments - 1U), 0, (Array) numArray1, destinationIndex8, 4);
      int destinationIndex9 = destinationIndex8 + 4;
      Array.Copy((Array) BitConverter.GetBytes(EndOfCentralDirectory), 0, (Array) numArray1, destinationIndex9, 8);
      int destinationIndex10 = destinationIndex9 + 8;
      Array.Copy((Array) BitConverter.GetBytes(numSegments), 0, (Array) numArray1, destinationIndex10, 4);
      int num6 = destinationIndex10 + 4;
      return numArray1;
    }

    private static int CountEntries(ICollection<ZipEntry> _entries)
    {
      int num = 0;
      foreach (ZipEntry entry in (IEnumerable<ZipEntry>) _entries)
      {
        if (entry.IncludedInMostRecentSave)
          ++num;
      }
      return num;
    }
  }
}
