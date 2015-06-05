
using System;

namespace scafftools.Model
{
    [Serializable]
    public enum UpdateDeleteAction
	{
		NoAction,
		Cascade,
		SetNull,
		SetDefault
	}
}
