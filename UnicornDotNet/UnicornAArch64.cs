using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace UnicornDotNet
{
    public class UnicornAArch64
    {
        internal readonly IntPtr uc;

        public IndexedProperty<int, ulong> X
        {
            get
            {
                return new IndexedProperty<int, ulong>(
                    (int i) => GetX(i),
                    (int i, ulong value) => SetX(i, value));
            }
        }

        public ulong LR
        {
            get { return GetRegister(Native.ArmRegister.LR); }
            set { SetRegister(Native.ArmRegister.LR, value); }
        }

        public ulong SP
        {
            get { return GetRegister(Native.ArmRegister.SP); }
            set { SetRegister(Native.ArmRegister.SP, value); }
        }

        public ulong PC
        {
            get { return GetRegister(Native.ArmRegister.PC); }
            set { SetRegister(Native.ArmRegister.PC, value); }
        }

        public UnicornAArch64()
        {
            Native.Interface.Checked(Native.Interface.uc_open((uint)Native.UnicornArch.UC_ARCH_ARM64, (uint)Native.UnicornMode.UC_MODE_LITTLE_ENDIAN, out uc));
        }

        ~UnicornAArch64()
        {
            Native.Interface.Checked(Native.Interface.uc_close(uc));
        }

        public void RunForCount(ulong count)
        {
            Native.Interface.Checked(Native.Interface.uc_emu_start(uc, this.PC, 0xFFFFFFFFFFFFFFFFu, 0, count));
        }

        public void Step()
        {
            RunForCount(1);
        }

        internal static Native.ArmRegister[] X_registers = new Native.ArmRegister[31]
        {
            Native.ArmRegister.X0,
            Native.ArmRegister.X1,
            Native.ArmRegister.X2,
            Native.ArmRegister.X3,
            Native.ArmRegister.X4,
            Native.ArmRegister.X5,
            Native.ArmRegister.X6,
            Native.ArmRegister.X7,
            Native.ArmRegister.X8,
            Native.ArmRegister.X9,
            Native.ArmRegister.X10,
            Native.ArmRegister.X11,
            Native.ArmRegister.X12,
            Native.ArmRegister.X13,
            Native.ArmRegister.X14,
            Native.ArmRegister.X15,
            Native.ArmRegister.X16,
            Native.ArmRegister.X17,
            Native.ArmRegister.X18,
            Native.ArmRegister.X19,
            Native.ArmRegister.X20,
            Native.ArmRegister.X21,
            Native.ArmRegister.X22,
            Native.ArmRegister.X23,
            Native.ArmRegister.X24,
            Native.ArmRegister.X25,
            Native.ArmRegister.X26,
            Native.ArmRegister.X27,
            Native.ArmRegister.X28,
            Native.ArmRegister.X29,
            Native.ArmRegister.X30,
        };

        internal ulong GetRegister(Native.ArmRegister register)
        {
            byte[] value_bytes = new byte[8];
            Native.Interface.Checked(Native.Interface.uc_reg_read(uc, (int)register, value_bytes));
            return (ulong)BitConverter.ToInt64(value_bytes, 0);
        }

        internal void SetRegister(Native.ArmRegister register, ulong value)
        {
            byte[] value_bytes = BitConverter.GetBytes(value);
            Native.Interface.Checked(Native.Interface.uc_reg_write(uc, (int)register, value_bytes));
        }

        public ulong GetX(int index)
        {
            Contract.Requires(index <= 30, "invalid register");

            return GetRegister(X_registers[index]);
        }

        public void SetX(int index, ulong value)
        {
            Contract.Requires(index <= 30, "invalid register");

            SetRegister(X_registers[index], value);
        }

        public byte[] MemoryRead(ulong address, ulong size)
        {
            byte[] value = new byte[size];
            Native.Interface.Checked(Native.Interface.uc_mem_read(uc, address, value, size));
            return value;
        }

        public byte   MemoryRead8 (ulong address) { return MemoryRead(address, 1)[0]; }
        public UInt16 MemoryRead16(ulong address) { return (UInt16)BitConverter.ToInt16(MemoryRead(address, 2), 0); }
        public UInt32 MemoryRead32(ulong address) { return (UInt32)BitConverter.ToInt32(MemoryRead(address, 4), 0); }
        public UInt64 MemoryRead64(ulong address) { return (UInt64)BitConverter.ToInt64(MemoryRead(address, 8), 0); }

        public void MemoryWrite(ulong address, byte[] value)
        {
            Native.Interface.Checked(Native.Interface.uc_mem_write(uc, address, value, (ulong)value.Length));
        }

        public void MemoryWrite8 (ulong address, byte value)   { MemoryWrite(address, new byte[]{value}); }
        public void MemoryWrite16(ulong address, Int16 value)  { MemoryWrite(address, BitConverter.GetBytes(value)); }
        public void MemoryWrite16(ulong address, UInt16 value) { MemoryWrite(address, BitConverter.GetBytes(value)); }
        public void MemoryWrite32(ulong address, Int32 value)  { MemoryWrite(address, BitConverter.GetBytes(value)); }
        public void MemoryWrite32(ulong address, UInt32 value) { MemoryWrite(address, BitConverter.GetBytes(value)); }
        public void MemoryWrite64(ulong address, Int64 value)  { MemoryWrite(address, BitConverter.GetBytes(value)); }
        public void MemoryWrite64(ulong address, UInt64 value) { MemoryWrite(address, BitConverter.GetBytes(value)); }

        public void MemoryMap(ulong address, ulong size, MemoryPermission permissions)
        {
            Native.Interface.Checked(Native.Interface.uc_mem_map(uc, address, size, (uint)permissions));
        }

        public void MemoryUnmap(ulong address, ulong size)
        {
            Native.Interface.Checked(Native.Interface.uc_mem_unmap(uc, address, size));
        }

        public void MemoryProtect(ulong address, ulong size, MemoryPermission permissions)
        {
            Native.Interface.Checked(Native.Interface.uc_mem_protect(uc, address, size, (uint)permissions));
        }

        public void DumpMemoryInformation()
        {
            Native.Interface.Checked(Native.Interface.uc_mem_regions(uc, out IntPtr regions_raw, out uint length));
            Native.Interface.MarshalArrayOf<Native.UnicornMemoryRegion>(regions_raw, (int)length, out var regions);
            foreach (var region in regions)
            {
                Console.WriteLine("region: begin {0:X16} end {1:X16} perms {2:X8}", region.begin, region.end, region.perms);
            }
        }
    }
}