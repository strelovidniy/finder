import IRoleNestedOption from './role-nested-option.interface';


interface IRoleOption {
    name: string;
    index: string;
    checked: boolean;
    expanded: boolean;
    options: IRoleNestedOption[];
}

export default IRoleOption;
