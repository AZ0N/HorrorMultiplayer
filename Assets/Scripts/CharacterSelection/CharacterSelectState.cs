using System;
using Unity.Netcode;

public struct CharacterSelecState : INetworkSerializable, IEquatable<CharacterSelecState>
{
    public ulong clientId;
    public int characterId;

    public CharacterSelecState(ulong clientId, int characterId = -1) {
        this.clientId = clientId;
        this.characterId = characterId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref characterId);
    }

    public bool Equals(CharacterSelecState other)
    {
        return clientId == other.clientId && characterId == other.characterId;
    }
}