# Distroir.BSP

Open-source library for reading and editing BSP files

## Features

Distroir.BSP library allows you to easily read and edit BSP files. Things you can do with this library:

- Reading BSP file header
- Reading lump informations
- Reading lump data
- Overwriting header
- Overwriting lump informations
- Overwriting lump data

## Code exampless

### Reading BSP file header

```csharp
using (BspReader reader = new BspReader(filename))
{
    BspInfo info = reader.ReadInfo();
}
```

### Reading lump informations

You can read lump informations from BSP file header:

```csharp
BspInfo info;
// Some code
using (var reader = new BspReader(filename))
{
    info = reader.ReadInfo();
}

BspLumpInfo lumpInfo = info.Lumps[40];
```

Or directly from file:

```csharp
using (var reader = new BspReader(filename))
{
    BspLumpInfo infoOne = reader.ReadLumpInfo(40);
    BspLumpInfo infoTwo = reader.ReadLumpInfo(BspLumpType.LUMP_PAKFILE);
}
```

#### Reading lump data

```csharp
using (BspReader reader = new BinaryReader(filename))
{
    byte[] lumpData = reader.ReadLumpData(BspLumpType.LUMP_PAKFILE);
}
```

### Writing lump informations

#### Overwriting BSP file header

```csharp
BspInfo info = new BspInfo();
// Some code
using (var writer = new BspInfoWriter(filename))
{
    writer.WriteInfo(info);
}

```

#### Overwriting BspLumpInfo

```csharp
BspLumpInfo info = new BspLumpInfo();
// Some code
using (var writer = new BspInfoWriter(filename))
{
    writer.WriteBspLumpInfo(info, BspLumpType.LUMP_PAKFILE);
}
```

#### Overwriting lump data

You can also overwrite lump data. Length of lump and offsets of other lumps will be updated:

```csharp
byte[] newLumpData = new byte[4] { 1, 36, 96, 2 };

using (var writer = new BspLumpDataWriter(oldFilename, newFilename))
{
    writer.WriteLumpData(BspLumpType.LUMP_PAKFILE, newLumpData);
}
```