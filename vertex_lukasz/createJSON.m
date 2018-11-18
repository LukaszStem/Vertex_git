% Assumes a workspace variable "Results" exists



instance = '2'

spikes_json = jsonencode(Results.spikes)
fid = fopen(strcat(instance,'spikes.json'), 'wt')
fprintf(fid, '%s', spikes_json);
fclose(fid)

LFP_json = jsonencode(Results.LFP)
fid = fopen(strcat(instance,'LFP.json'), 'wt')
fprintf(fid, '%s', LFP_json);
fclose(fid)

params_json = jsonencode(Results.params)
fid = fopen(strcat(instance,'params.json'), 'wt')
fprintf(fid, '%s', params_json);
fclose(fid)

