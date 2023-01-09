﻿public class ModuleCreator : IModuleCreator
{
    private readonly byte[] m_Bytes;
    private readonly IErrorListener m_ErrorListener;

    public ModuleCreator(byte[] bytes, IErrorListener errorListener)
    {
        m_Bytes = bytes;
        m_ErrorListener = errorListener;
    }

    public ModuleCreationResult Create()
    {
        var moduleReaderParameters = new ModuleReaderParameters(m_ErrorListener);
        var module = SerializedModuleDefinition.FromBytes(m_Bytes, moduleReaderParameters);
        var managedPEImageBuilder = new ManagedPEImageBuilder(MetadataBuilderFlags.PreserveAll);

        module.Attributes &= ~DotNetDirectoryFlags.ILOnly;

        var hasX86 = module.MachineType == MachineType.I386; 
        if (hasX86)
        {
            module.PEKind = OptionalHeaderMagic.PE32;
            module.MachineType = MachineType.I386;
            module.Attributes |= DotNetDirectoryFlags.Bit32Required;
        }
        else
        {
            module.PEKind = OptionalHeaderMagic.PE32Plus;
            module.MachineType = MachineType.Amd64;
        }

        return new ModuleCreationResult
        {
            Module = module,
            ModuleReaderParameters = moduleReaderParameters,
            PEImageBuilder = managedPEImageBuilder,
        };
    }
}