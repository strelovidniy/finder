interface ICreateRolerequest {
    name: string;
    type: string;
    canDeleteUsers?: boolean;
    canRestoreUsers?: boolean;
    canEditUsers?: boolean;
    canCreateRoles?: boolean;
    canEditRoles?: boolean;
    canDeleteRoles?: boolean;
    canSeeAllUsers?: boolean;
    canSeeAllRoles?: boolean;
    canSeeRoles?: boolean;
    canMaintainSystem?: boolean;
    canSeeQuestions?: boolean;
    canCreateQuestion?: boolean;
    canEditQuestion?: boolean;
    canDeleteQuestion?: boolean;
    canSeeWidgetConfigs?: boolean;
    canCreateWidgetConfig?: boolean;
    canEditWidgetConfig?: boolean;
    canDeleteWidgetConfig?: boolean;
    canSeeWidgetInstances?: boolean;
    canCreateWidgetInstance?: boolean;
    canEditWidgetInstance?: boolean;
    canDeleteWidgetInstance?: boolean;
    canImportProducts?: boolean;
    canSyncProducts?: boolean;
}

export default ICreateRolerequest;
