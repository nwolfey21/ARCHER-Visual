function [ v, vn, f ] = splitOBJ( objFile )
%This function takes in an imported .obj file and outputs separate vectors
%for the vertex data, vertex normal data, and the face data.
vn = 0
vIndx  = 1;
vnIndx = 1;
fIndx  = 1;
for i=1:length(objFile)
    if(objFile(i,1) == 1)
        v(vIndx,:) = objFile(i,:);
        vIndx = vIndx+1;
    elseif(objFile(i,1) == 2)
        vn(vnIndx,:) = objFile(i,:);
        vnIndx = vnIndx+1;
    elseif(objFile(i,1) == 3)
        f(fIndx,:) = objFile(i,:);
        fIndx = fIndx+1;
    end
end

end