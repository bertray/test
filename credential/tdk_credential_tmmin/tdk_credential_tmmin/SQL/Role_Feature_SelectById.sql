select 
	b.feature_id as "Id",
	b.feature_name as "Name"
from tb_m_role_feature a
inner join tb_m_feature b on a.feature_id = b.feature_id
where (a.role_id = @RoleId) and (a.function_id = @FunctionId)