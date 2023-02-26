﻿namespace BitMono.Protections;

[DoNotResolve(MemberInclusionFlags.SpecialRuntime)]
public class BitMethodDotnet : Protection
{
    private readonly Random _random;

    public BitMethodDotnet(RuntimeImplementations runtime, ProtectionContext context) : base(context)
    {
        _random = runtime.Random;
    }

    public override Task ExecuteAsync(ProtectionParameters parameters)
    {
        foreach (var method in parameters.Members.OfType<MethodDefinition>())
        {
            if (method is { CilMethodBody: { } body, IsConstructor: false })
            {
                var randomMethodBodyIndex = 0;
                if (body.Instructions.Count >= 3)
                {
                    randomMethodBodyIndex = _random.Next(0, body.Instructions.Count);
                }

                var randomValue = _random.Next(0, 3);
                var randomOpCode = randomValue switch
                {
                    0 => CilOpCodes.Readonly,
                    1 => CilOpCodes.Unaligned,
                    2 => CilOpCodes.Volatile,
                    3 => CilOpCodes.Constrained,
                    _ => throw new ArgumentOutOfRangeException(),
                };

                var label = body.Instructions[randomMethodBodyIndex].CreateLabel();
                body.Instructions.Insert(randomMethodBodyIndex, new CilInstruction(CilOpCodes.Br_S));
                if (randomOpCode == CilOpCodes.Unaligned)
                {
                    body.Instructions.Insert(randomMethodBodyIndex + 1, new CilInstruction(randomOpCode, (sbyte)0));
                }
                else
                {
                    body.Instructions.Insert(randomMethodBodyIndex + 1, new CilInstruction(randomOpCode));
                }
                body.Instructions[randomMethodBodyIndex].Operand = label;
            }
        }
        return Task.CompletedTask;
    }
}