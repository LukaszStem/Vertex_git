% Assumes a workspace variable "Results" exists
function [] =  createJson(filename, object)

fprintf("Ecoding object for %s\n", filename);
jsonText = jsonencode(object);
fprintf("Creating and writing to %s\n", filename);
fid = fopen(filename, 'wt');
fprintf(fid, '%s', jsonText);
fclose(fid);
fprintf("Finshed writing to %s\n\n", filename);

end