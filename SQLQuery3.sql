SELECT 
	COMP.[Id], 
	COMP.[Assunto], 
	COMP.[Data],
	COMP.[HoraInicio], 
	COMP.[HoraTermino],
	COMP.[Tipo],
	COMP.[Local],
	COMP.[Link],
	COMP.[Contato_Id],
	CT.[Nome],
	CT.[Email],
	CT.[Telefone],
	CT.[Cargo],
	CT.[Empresa]
FROM
	[TBCompromisso] as COMP LEFT JOIN 
	[TBContato] as CT
ON
	CT.[Id] = COMP.Contato_Id