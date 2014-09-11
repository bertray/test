select	 company_code as Id
		,company_name as Name
		,company_abbreviation as Alias
		,company_type as _Type
from tb_m_company
where company_code = @Id